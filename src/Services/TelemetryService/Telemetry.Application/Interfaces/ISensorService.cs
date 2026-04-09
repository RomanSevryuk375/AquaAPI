using Contracts.Events.SensorEvents;

namespace Telemetry.Application.Interfaces;

public interface ISensorService
{
    Task CreateSensorFromEventAsync(
        SensorCreatedEvent sensorCreated, 
        CancellationToken cancellationToken);

    Task DeleteSensorFromEventAsync(
        SensorDeletedEvent sensorDeleted, 
        CancellationToken cancellationToken);

    Task UpdateSensorFromEventAsync(
        SensorUpdatedEvent updatedEvent, 
        CancellationToken cancellationToken);

    Task SetSensorStateFromEventAsync(
        SensorStateChangedEvent sensorStateChanged,
        CancellationToken cancellationToken);
}