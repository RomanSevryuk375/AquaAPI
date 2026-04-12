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
        ChangeRelayStateCommand relayState,
        CancellationToken cancellationToken);

    Task ChangedModeFromEventAsync(
        ChangeRelayModeCommand relayMode,
        CancellationToken cancellationToken);
}
