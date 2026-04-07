using Contracts.Enums;

namespace Device.Application.DTOs.Relay;

public record RelayUpdateRequestDto
{
    public Guid Id { get; init; }
    public Guid ControllerId { get; init; }
    public string HardwarePin { get; init; } = string.Empty;
    public RelayPurposeEnum Purpose { get; init; }
}
