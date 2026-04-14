namespace IdentityService.Application.DTOs;

public class LoginUserRequestDto
{
    public string Email { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
