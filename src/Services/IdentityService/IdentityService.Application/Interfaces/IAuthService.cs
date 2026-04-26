using IdentityService.Application.DTOs;

namespace IdentityService.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(
        LoginUserRequestDto loginUser, 
        CancellationToken cancellationToken);

    Task<LoginResponseDto> LoginWithRefreshTokenAsync(
        RefreshTokenRequestDto request,
        CancellationToken cancellationToken);

    Task LogoutAsync(CancellationToken cancellationToken);

    Task<LoginResponseDto> RegisterUserAsync(
        RegisterUserRequestDto registerDto, 
        CancellationToken cancellationToken);
}