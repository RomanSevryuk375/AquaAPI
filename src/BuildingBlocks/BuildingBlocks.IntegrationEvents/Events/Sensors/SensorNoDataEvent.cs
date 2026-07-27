using BuildingBlocks.Domain.Enums;

namespace BuildingBlocks.IntegrationEvents.Events.Sensors;

public sealed record SensorNoDataEvent : BaseIntegrationEvent
{
    public Guid SensorId { get; init; }
    public SensorState State { get; init; }
    public DateTime LastSeenAt { get; init; }
}
