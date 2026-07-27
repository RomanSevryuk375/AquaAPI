using BuildingBlocks.Domain.Results;
using Device.Application.Extesions;
using MassTransit;
using Microsoft.Extensions.Options;

namespace Device.Application.Features.RelayCommands.Command.ToggleRelayState;

public sealed class ToggleRelayStateHandler(
    IRelayRepository relayRepository,
    IRelayCommandsRepository queueRepository,
    IOptions<DeviceSettings> deviceOptions) : IRequestHandler<ToggleRelayStateCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        ToggleRelayStateCommand request,
        CancellationToken cancellationToken)
    {
        Relay? existingRelay = await relayRepository.GetByIdAsync(
            request.RelayId, cancellationToken);

        existingRelay!.SetState(!existingRelay.IsActive);

        Result<RelayCommand> newCommand = RelayCommand.Create(
            id: NewId.NextGuid(), existingRelay.ControllerId, existingRelay.Id,
            existingRelay.IsActive,
            expireAt: DateTime.UtcNow.AddMinutes(deviceOptions.Value.CommandTtlMinutes));
        if (newCommand.IsFailure)
        {
            return Result<bool>.Failure(newCommand.Error);
        }

        await queueRepository.AddAsync(newCommand.Value, cancellationToken);

        return Result<bool>.Success(existingRelay.IsActive);
    }
}
