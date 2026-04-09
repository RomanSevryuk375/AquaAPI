using Contracts.Enums;

namespace Control.Domain.SpecificationParams;

public record RelayFilterParams
{
    public Guid? AquariumId { get; init; }
    public RelayPurposeEnum? Purpose { get; init; }
    public bool? IsManual { get; init; }
    public bool? IsActive { get; init; }
}
