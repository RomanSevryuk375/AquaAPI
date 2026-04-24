using IdentityService.Application.DTOs;

namespace IdentityService.Application.Interfaces;

public interface IAuthService
{
    Task<string> CheckLoginAsync(
        LoginUserRequestDto loginUser,
        CancellationToken cancellationToken);

    Task<string> RegisterUserAsync(
        RegisterUserRequestDto registerDto,
        CancellationToken cancellationToken);
}