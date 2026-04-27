using AutoMapper;
using Contracts.Enums;
using Contracts.Events.RelayEvents;
using Contracts.Exceptions;
using Contracts.Extensions;
using Device.Application.DTOs.RelayCommands;
using Device.Application.Interfaces;
using Device.Domain.Entities;
using Device.Domain.Factories;
using Device.Domain.Interfaces;
using MassTransit;

namespace Device.Application.Services;

public class RelayCommandQueueService(
    IRelayRepository relayRepository,
    IControllerRepository controllerRepository,
    IRelayCommandsQueueRepository queueRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork,
    IPublishEndpoint publisherEndpoint) : IRelayCommandQueueService
{
    public async Task<IReadOnlyList<RelayCommandResponseDto>> GetPendingCommands(
        Guid controllerId,
        CancellationToken cancellationToken)
    {
        var commands = await queueRepository
            .GetPendingByControllerIdAsync(controllerId, cancellationToken);

        foreach (var command in commands)
        {
            command.MarkAsSent();
            await queueRepository.UpdateAsync(command, cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<IReadOnlyList<RelayCommandResponseDto>>(commands);
    }

    public async Task MarkAsCompletedByIdAsync(
        Guid commandId,
        CancellationToken cancellationToken)
    {
        var command = await queueRepository
            .GetByIdAsync(commandId, cancellationToken)
            ?? throw new NotFoundException($"Command {commandId} not found");

        var existingRelay = await relayRepository
            .GetByIdAsync(command.RelayId, cancellationToken)
            ?? throw new NotFoundException($"Relay {command.RelayId} not found");

        existingRelay.SetState(StateEvaluatorFactory.EvaluateEnum(command.Action));
        await relayRepository.UpdateAsync(existingRelay, cancellationToken);

        if (command.Status == CommandStatusEnum.Completed)
        {
            return;
        }

        command.MarkAsCompleted();

        await queueRepository.UpdateAsync(command, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task MarkAsFailedByIdAsync(
        Guid commandId,
        string errorMessage,
        CancellationToken cancellationToken)
    {
        var command = await queueRepository
            .GetByIdAsync(commandId, cancellationToken)
            ?? throw new NotFoundException($"Command {commandId} not found");

        if (command.Status == CommandStatusEnum.Failed)
        {
            return;
        }

        command.MarkAsFailed(errorMessage);

        await queueRepository.UpdateAsync(command, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> SetRelayStateAsync(
    ChangeRelayStateCommand command,
    CancellationToken cancellationToken)
    {
        var existingRelay = await relayRepository
            .GetByIdAsync(command.RelayId, cancellationToken);

        if (existingRelay is null)
        {
            return Result
                .Failure($"Relay {command.RelayId} not found", false);
        }

        var existingController = await controllerRepository
            .GetByIdAsync(command.ControllerId, cancellationToken);

        if (existingController is null)
        {
            return Result
                .Failure($"Controller {command.ControllerId} not found", false);
        }

        if (existingRelay.IsManual ||
           (command.ExpireAt.HasValue && command.ExpireAt.Value < DateTime.UtcNow) ||
           existingRelay.IsActive == StateEvaluatorFactory.EvaluateEnum(command.Action))
        {
            return Result
                .Failure($"Command is unavalible or was expired.", false);
        }

        var (newCommand, errors) = RelayCommandsQueueEntity.Create(
            existingController.Id,
            existingRelay.Id,
            command.Action,
            command.ExpireAt);

        if (newCommand is null)
        {
            return Result
                .Failure($"Failed to create {nameof(RelayCommandsQueueEntity)}: " +
                $"{string.Join(", ", errors!)}");
        }

        try
        {
            await queueRepository.UpdateAsync(newCommand, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result
                .Failure(ex.Message, retryable: true);
        }
    }

    public async Task<bool> ToggleRelayModeAsync(
        Guid relayId,
        CancellationToken cancellationToken)
    {
        var existingRelay = await relayRepository
            .GetByIdAsync(relayId, cancellationToken)
            ?? throw new NotFoundException($"Relay {relayId} not found");

        existingRelay.ToggleMode();

        await relayRepository.UpdateAsync(existingRelay, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await publisherEndpoint.Publish(new ChangeRelayModeCommand
        {
            RelayId = existingRelay.Id,
            IsManual = existingRelay.IsManual,
        }, cancellationToken);

        return existingRelay.IsManual;
    }

    public async Task<bool> ToggleRelayStateAsync(
        Guid relayId,
        CancellationToken cancellationToken)
    {
        var existingRelay = await relayRepository
            .GetByIdAsync(relayId, cancellationToken)
            ?? throw new NotFoundException($"Relay {relayId} not found");

        existingRelay.ToggleState();

        await relayRepository.UpdateAsync(existingRelay, cancellationToken);

        var (newCommand, errors) = RelayCommandsQueueEntity.Create(
            existingRelay.ControllerId,
            existingRelay.Id,
            StateEvaluatorFactory.EvaluateBool(existingRelay.IsActive),
            DateTime.UtcNow.AddMinutes(15));

        if (newCommand is null)
        {
            throw new DomainValidationException(
                $"Failed to create {nameof(RelayCommandsQueueEntity)}: {string.Join(", ", errors!)}");
        }

        await queueRepository.AddAsync(newCommand, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return existingRelay.IsActive;
    }
}
