namespace BuildingBlocks.IntegrationEvents.Events.Telemetrys;

public sealed record TelemetryBatchEventItem : BaseIntegrationEvent
{
    public Guid SensorId { get; init; }
    public double Value { get; init; }
    public string ExternalMessageId { get; init; } = string.Empty;
    public DateTime RecordedAt { get; init; }
}
