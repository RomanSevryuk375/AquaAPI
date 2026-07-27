namespace BuildingBlocks.IntegrationEvents.Events.Telemetrys;

public sealed record TelemetryReceivedEvent : BaseIntegrationEvent
{
    public Guid SensorId { get; init; }
    public double Value { get; init; }
    public DateTime RecordedAt { get; init; }
}
