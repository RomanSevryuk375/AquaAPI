// Ignore Spelling: Dto

namespace Control.Application.DTOs.Ecosystems;

public sealed record EcosystemUpdateRequestDto
{
    public string Name { get; init; } = string.Empty;
    public double Volume { get; init; }
}
