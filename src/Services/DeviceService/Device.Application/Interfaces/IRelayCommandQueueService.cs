using Contracts.Events.RelayEvents;
using Contracts.Extensions;
using Device.Application.DTOs.RelayCommands;

namespace Device.Application.Interfaces;

public interface IRelayCommandQueueService
{
    Task<IReadOnlyList<RelayCommandResponseDto>> GetPendingCommands(
        Guid controllerId, 
        CancellationToken cancellationToken);

    Task MarkAsCompletedByIdAsync(
        Guid commandId, 
        CancellationToken cancellationToken);

    Task MarkAsFailedByIdAsync(
        Guid commandId, 
        string errorMessage, 
        CancellationToken cancellationToken);

    Task<Result> SetRelayStateAsync(
        ChangeRelayStateCommand command, 
        CancellationToken cancellationToken);

    Task<bool> ToggleRelayModeAsync(
        Guid relayId, 
        CancellationToken cancellationToken);

    Task<bool> ToggleRelayStateAsync(
        Guid relayId, 
        CancellationToken cancellationToken);
}