using Contracts.Enums;

namespace Telemetry.Application.DTOs;

public record SensorRequestDto
{
    public Guid ControllerId { get; init; }
    public SensorTypeEnum Type { get; init; }
    public string Unit { get; init; } = string.Empty;
    public double LastValue { get; init; }
    public DateTime UpdatedAt { get; init; }
}
