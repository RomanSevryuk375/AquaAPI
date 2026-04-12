namespace Control.Application.DTOs.Aquarium;

public record AquariumUpdateRequestDto
{
    public string Name { get; init; } = string.Empty;
}
