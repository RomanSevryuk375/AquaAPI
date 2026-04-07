using Contracts.Enums;

namespace Device.Application.DTOs.Sensor;

public record SensorResponseDto
{
    public Guid Id { get; init; }
    public Guid ControllerId { get; init; }
    public string HardwarePin { get; init; } = string.Empty;
    public SensorTypeEnum Type { get; init; }
    public SensorStateEnum State { get; private set; }
    public string Unit { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}
