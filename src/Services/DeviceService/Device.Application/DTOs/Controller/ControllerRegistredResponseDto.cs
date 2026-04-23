namespace Device.Application.DTOs.Controller;

public record ControllerRegistredResponseDto
{
    public Guid ControllerId { get; init; }
    public string DeviceToken { get; init; } = string.Empty;
}
