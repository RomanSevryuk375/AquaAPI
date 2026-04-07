using Contracts.Enums;

namespace Device.Application.DTOs.Relay;

public record RelayResponseDto
{
    public Guid Id { get; init; }
    public Guid ControllerId { get; init; }
    public string HardwarePin { get; init; } = string.Empty;
    public RelayPurposeEnum Purpose { get; init; }
    public bool IsActive { get; init; }
    public bool IsManual { get; init; }
    public DateTime CreatedAt { get; init; }
}
