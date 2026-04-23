using Contracts.Events.AquariumEvents;

namespace Notification.Application.Interfaces;

public interface IAquariumServiceFromEvent
{
    Task CreateAquariumFromEventAsync(
        AquariumCreatedEvend aquariumCreated, 
        CancellationToken cancellationToken);

    Task DeleteAquariumFromEventAsync(
        AquariumDeletedEvent aquariumDeleted, 
        CancellationToken cancellationToken);

    Task UpdateAquariumFromEventAsync(
        AquarimUdatedEvent aquarimUdated, 
        CancellationToken cancellationToken);
}