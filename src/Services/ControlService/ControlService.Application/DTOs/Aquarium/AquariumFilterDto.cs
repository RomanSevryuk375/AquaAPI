namespace Control.Application.DTOs.Aquarium;

public record AquariumFilterDto
{
    public string? Name { get; init; } = string.Empty;
    public Guid? ControllerId { get; init; }
}
