namespace Control.Application.DTOs.Aquarium;

public record AquariumRequestDto
{
    public string Name { get; init; } = string.Empty;
    public Guid ControllerId { get; init; }
}
