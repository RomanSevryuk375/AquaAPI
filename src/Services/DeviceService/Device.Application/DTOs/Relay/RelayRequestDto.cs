using Contracts.Enums;

namespace Device.Application.DTOs.Relay;

public record RelayRequestDto
{
    public Guid ControllerId { get; init; }
    public string HardwarePin { get; init; } = string.Empty;
    public RelayPurposeEnum Purpose { get; init; }
    public bool IsActive { get; init; }
    public bool IsManual { get; init; }
}
