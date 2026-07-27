using BuildingBlocks.Domain.Enums;

namespace BuildingBlocks.IntegrationEvents.Events.Sensors;

public sealed record SensorStateChangedEvent : BaseIntegrationEvent
{
    public Guid SensorId { get; init; }
    public SensorState State { get; init; }
}
