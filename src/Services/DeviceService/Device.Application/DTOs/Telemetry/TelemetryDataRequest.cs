namespace Device.Application.DTOs.Telemetry;

public record TelemetryDataRequest
{
    public string MacAdress { get; init; } = string.Empty;
    public Guid SensorId { get; init; }
    public double Value { get; init; }
    public string ExternalMessageId { get; init; } = string.Empty;
    public DateTime RecordedAt { get; init; }
}
