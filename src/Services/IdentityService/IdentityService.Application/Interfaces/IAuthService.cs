using IdentityService.Application.DTOs;

namespace IdentityService.Application.Interfaces;

public interface IAuthService
{
    Task<string> CheckLoginAsync(LoginUserRequestDto loginUser);
    Task<string> RegisterUserAsync(RegisterUserRequestDto registerDto);
}