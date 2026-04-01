namespace Telemetry.Application.DTOs;

public record TelemetryDataRequest
{
    public Guid SensorId { get; init; }
    public double Value { get; init; }
    public string ExternalMessageId { get; init; }
    public DateTime RecordedAt { get; init; }
}
