namespace Contracts.Events.TelemetryEvents;

public record TelemetryReportedFromHardwareEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public Guid SensorId { get; init; }
    public double Value { get; init; }
    public string ExternalMessageId { get; init; } = string.Empty;
    public DateTime RecordedAt { get; init; }
}
