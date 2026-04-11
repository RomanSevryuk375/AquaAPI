using Contracts.Events.SensorEvents;

namespace Control.Application.Interfaces;

public interface ISensorServiceFromEvent
{
    Task CreateSensorFromEventAsync(
        SensorCreatedEvent sensorCreated,
        CancellationToken cancellationToken);

    Task UpdatedSensorFromEventAsync(
        SensorUpdatedEvent sensorUpdated,
        CancellationToken cancellationToken);

    Task DeletedSensorFromEventAsync(
        SensorDeletedEvent relayDeleted,
        CancellationToken cancellationToken);

    Task ChangedStateFromEventAsync(
        SensorStateChangedCommand relayState,
        CancellationToken cancellationToken);

    Task HandleSensorNoDataEvent(
        SensorNoDataEvent relayMode,
        CancellationToken cancellationToken);
}
