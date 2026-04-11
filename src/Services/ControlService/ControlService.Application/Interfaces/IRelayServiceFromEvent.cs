using Contracts.Events.RelayEvents;

namespace Control.Application.Interfaces;

public interface IRelayServiceFromEvent
{
    Task CreateRelayFromEventAsync(
        RelayCreatedEvent relayCreated, 
        CancellationToken cancellationToken);

    Task UpdatedRelayFromEventAsync(
        RelayUpdatedEvent relayUpdated, 
        CancellationToken cancellationToken);

    Task DeletedRelayFromEventAsync(
        RelayDeletedEvent relayDeleted, 
        CancellationToken cancellationToken);

    Task ChangedStateFromEventAsync(
        RelayStateChangedCommand relayState,
        CancellationToken cancellationToken);

    Task ChangedModeFromEventAsync(
        RelayModeChangedCommand relayMode,
        CancellationToken cancellationToken);
}
