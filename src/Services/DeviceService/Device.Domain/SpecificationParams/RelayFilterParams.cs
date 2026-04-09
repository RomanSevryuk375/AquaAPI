using Contracts.Enums;

namespace Device.Domain.SpecificationParams;

public record RelayFilterParams
{
    public Guid? ControllerId { get; init; }
    public RelayPurposeEnum? Purpose { get; init; }
    public bool? IsActive { get; init; }
    public bool? IsManual { get; init; }
}
