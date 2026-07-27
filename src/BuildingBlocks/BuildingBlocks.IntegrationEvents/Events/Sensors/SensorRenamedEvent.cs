namespace BuildingBlocks.IntegrationEvents.Events.Sensors;

public sealed record SensorRenamedEvent : BaseIntegrationEvent
{
    public Guid SensorId { get; init; }
    public string Name { get; init; } = string.Empty;
}
