namespace Control.Domain.SpecificationParams;

public record ScheduleFilterParams
{
    public Guid? AquariumId { get; init; }
    public Guid? RelayId { get; init; }
    public bool? IsFadeMode { get; init; }
    public bool? IsEnable { get; init; }
}
