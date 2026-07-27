namespace BuildingBlocks.IntegrationEvents.Events.Sensors;

public sealed record SensorDeletedEvent : BaseIntegrationEvent
{
    public Guid SensorId { get; init; }
}
