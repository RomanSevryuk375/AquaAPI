namespace Device.Domain.SpecificationParams;

public record ControllerFilterParams
{
    public string? SearchTerm { get; init; }
    public bool? IsOnline { get; init; }
}
