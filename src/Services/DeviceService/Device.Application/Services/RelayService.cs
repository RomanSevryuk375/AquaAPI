using Contracts.Events.RelayEvents;
using Contracts.Exceptions;
using Device.Application.DTOs.Relay;
using Device.Application.Interfaces;
using Device.Domain.Entities;
using Device.Domain.Interfaces;
using Device.Domain.SpecificationParams;
using Device.Domain.Specifications;
using MassTransit;

namespace Device.Application.Services;

public class RelayService(
    IControllerRepository controllerRepository,
    IRelayRepository relayRepository,
    IUnitOfWork unitOfWork,
    IPublishEndpoint publisherEndpoint) : IRelayService
{
    public async Task<Guid> AddRelayAsync(
        RelayRequestDto request, 
        CancellationToken cancellationToken)
    {
        var existingController = await controllerRepository
            .GetByIdAsync(request.ControllerId, cancellationToken)
            ?? throw new NotFoundException($"Controller {request.ControllerId} not found");

        var (relay, errors) = RelayEntity.Create(
            request.ControllerId,
            request.HardwarePin,
            request.Purpose,
            request.IsActive,
            request.IsManual);

        if (relay is null)
        {
            throw new DomainValidationException(
                $"Failed to create {nameof(RelayEntity)}: {string.Join(", ", errors!)}");
        }

        var result = await relayRepository.AddAsync(relay, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await publisherEndpoint.Publish(new RelayCreatedEvent
        {
            ControllerId = request.ControllerId,
            RelayId = relay.Id,
            Purpose = relay.Purpose,
            IsManual = relay.IsActive,
            IsActive = relay.IsActive,
            CreatedAt = relay.CreatedAt,
        }, cancellationToken);

        return result;
    }

    public async Task DeleteRelayAsync(
        Guid id, 
        CancellationToken cancellationToken)
    {
        await relayRepository.DeleteAsync(id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await publisherEndpoint.Publish(new RelayDeletedEvent
        {
            RelayId = id,
        }, cancellationToken);
    }

    public async Task<IReadOnlyList<RelayResponseDto>> GetAllRelaysAsync(
        RelayFilterDto filter, 
        int? skip, 
        int? take, 
        CancellationToken cancellationToken)
    {
        var specification = new RelayFilterSpecification(
            new RelayFilterParams
            {
                ControllerId = filter.ControllerId,
                Purpose = filter.Purpose,
                IsActive = filter.IsActive, 
                IsManual = filter.IsManual,
            });

        var relays = await relayRepository.GetAllAsync(
            specification, 
            skip, 
            take, 
            cancellationToken);

        return relays.Select(x => new RelayResponseDto
        {
            Id = x.Id,
            ControllerId = x.ControllerId,
            HardwarePin = x.HardwarePin,
            Purpose = x.Purpose,
            IsActive = x.IsActive,
            IsManual = x.IsManual,
            CreatedAt = x.CreatedAt,
        }).ToList();
    }

    public async Task<RelayResponseDto> GetRelayByIdAsync(
        Guid id, 
        CancellationToken cancellationToken)
    {
        var existingRelay = await relayRepository
            .GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Relay {id} not found");

        return new RelayResponseDto
        {
            Id = existingRelay.Id,
            ControllerId = existingRelay.ControllerId,
            HardwarePin = existingRelay.HardwarePin,
            Purpose = existingRelay.Purpose,
            IsActive = existingRelay.IsActive,
            IsManual = existingRelay.IsManual,
            CreatedAt = existingRelay.CreatedAt,
        };
    }

    public async Task<bool> SetRelayStateAsync(
        Guid id, 
        bool state, 
        CancellationToken cancellationToken)
    {
        var existingRelay = await relayRepository
            .GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Relay {id} not found");

        if (existingRelay.IsManual)
        {
            return existingRelay.IsActive;
        }

        existingRelay.SetState(state);

        await relayRepository.UpdateAsync(existingRelay, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await publisherEndpoint.Publish(new ChangeRelayStateCommand
        {
            RelayId = existingRelay.Id,
            IsActive = existingRelay.IsActive,
        }, cancellationToken);

        return existingRelay.IsActive;
    }

    public async Task SetRelayStateFromCommandAsync(
        ChangeRelayStateCommand command,
        CancellationToken cancellationToken)
    {
        var relay = await relayRepository
            .GetByIdAsync(command.RelayId, cancellationToken);

        if (relay is null || relay.IsManual)
        {
            return;
        }

        relay.SetState(command.IsActive);

        await relayRepository.UpdateAsync(relay, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ToggleRelayModeAsync(
        Guid id, 
        CancellationToken cancellationToken)
    {
        var existingRelay = await relayRepository
            .GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Relay {id} not found");

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
        Guid id, 
        CancellationToken cancellationToken)
    {
        var existingRelay = await relayRepository
            .GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Relay {id} not found");

        existingRelay.ToggleState();

        await relayRepository.UpdateAsync(existingRelay, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await publisherEndpoint.Publish(new ChangeRelayStateCommand
        {
            RelayId = existingRelay.Id,
            IsActive = existingRelay.IsActive,
        }, cancellationToken);

        return existingRelay.IsActive;
    }

    public async Task UpdateRelayAsync(
        Guid id,
        RelayUpdateRequestDto updateRequestDto, 
        CancellationToken cancellationToken)
    {
        var existingRelay = await relayRepository
            .GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(
                $"{nameof(RelayEntity)} {id} not found");

        var controller = await controllerRepository
            .GetByIdAsync(updateRequestDto.ControllerId, cancellationToken)
            ?? throw new NotFoundException(
                $"{nameof(ControllerEntity)} {updateRequestDto.ControllerId} not found");

        var errors = existingRelay.Update(
            updateRequestDto.ControllerId,
            updateRequestDto.HardwarePin,
            updateRequestDto.Purpose);

        if (errors is not null && errors.Count > 0)
        {
            throw new DomainValidationException(
                $"Update failed: {string.Join(", ", errors)}");
        }

        await relayRepository.UpdateAsync(existingRelay, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await publisherEndpoint.Publish(new RelayUpdatedEvent
        {
            RelayId= existingRelay.Id,
            ControllerId = existingRelay.ControllerId,
            Purpose = existingRelay.Purpose,
            IsActive = existingRelay.IsActive,
            IsManual = existingRelay.IsManual,
            CreatedAt = existingRelay.CreatedAt,
        }, cancellationToken);
    }
}
