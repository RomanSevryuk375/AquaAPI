namespace Telemetry.Application.DTOs;

public record TelemetryDataResponse
{

    public Guid Id { get; init; }
    public Guid SensorId { get; init; }
    public double Value { get; init; }
    public string ExternalMessageId { get; init; } = string.Empty;
    public DateTime RecordedAt { get; init; }
    public DateTime CreatedAt { get; init; }
}
