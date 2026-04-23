namespace Device.Application.DTOs.Telemetry;

public record TelemetryResponse
{
    public int AcceptedCount { get; init; }
    public List<string> ValidationErrors { get; init; } = [];
    public int SkippedCount { get; init; }
}
