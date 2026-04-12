namespace Control.Application.DTOs.Aquarium;

public record AquariumResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid ControllerId { get; init; }
    public DateTime CreatedAt { get; init; }
}
