using IdentityService.Application.DTOs;
using IdentityService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers;

[ApiController]
[Route("api/identity/v1/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<LoginResponseDto>> RegisterAsync(
        [FromBody] RegisterUserRequestDto request,
        CancellationToken cancellationToken)
    {
        var token = await authService
            .RegisterUserAsync(request, cancellationToken);

        return Ok(token);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> LoginAsync(
        [FromBody] LoginUserRequestDto request,
        CancellationToken cancellationToken)
    {
        var token = await authService
            .LoginAsync(request, cancellationToken);

        return Ok(token);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<LoginResponseDto>> RefreshAsync(
    [FromBody] string refreshToken,
    CancellationToken cancellationToken)
    {
        var result = await authService
            .LoginWithRefreshTokenAsync(refreshToken, cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> LogoutAsync(CancellationToken cancellationToken)
    {
        await authService.LogoutAsync(cancellationToken);

        return NoContent();
    }
}
