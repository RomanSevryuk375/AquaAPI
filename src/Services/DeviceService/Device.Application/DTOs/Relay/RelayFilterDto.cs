using Contracts.Enums;

namespace Device.Application.DTOs.Relay;

public record RelayFilterDto
{
    public Guid? ControllerId { get; init; }
    public RelayPurposeEnum? Purpose { get; init; }
    public bool? IsActive { get; init; }
    public bool? IsManual { get; init; }
}
