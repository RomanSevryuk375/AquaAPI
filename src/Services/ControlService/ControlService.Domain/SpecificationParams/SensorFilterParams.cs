using Contracts.Enums;

namespace Control.Domain.SpecificationParams;

public record SensorFilterParams
{
    public Guid? AquariumId { get; init; }
    public SensorStateEnum? State { get; init; }
    public SensorTypeEnum? Type { get; init; }
}
