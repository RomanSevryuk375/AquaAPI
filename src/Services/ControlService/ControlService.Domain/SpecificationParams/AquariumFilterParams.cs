namespace Control.Domain.SpecificationParams;

public record AquariumFilterParams
{
    public Guid? UserId { get; init; }
    public string? Name { get; init; } = string.Empty;
    public Guid? ControllerId { get; init; }
}
