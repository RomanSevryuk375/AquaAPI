namespace Telemetry.Domain.SpecificationParams;

public record TelemetryFilterParams
{
    public Guid? SensorId { get; init; }
    public DateTime? From { get; init; }
    public DateTime? To { get; init; }
}
