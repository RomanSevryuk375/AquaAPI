namespace Device.Application.DTOs.Controller;

public record ControllerPingResponseDto
{
    public DateTime ServerTimeUtc { get; init; } = DateTime.UtcNow;
}
