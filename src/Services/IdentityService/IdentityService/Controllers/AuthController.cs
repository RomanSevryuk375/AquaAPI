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

        var loginResponse = new LoginResponseDto();

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddHours(12)
        };

        Response.Cookies.Append("AccessToken", loginResponse.AccessToken, cookieOptions);

        var refreshOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(30)
        };
        Response.Cookies.Append("RefreshToken", loginResponse.RefreshToken, refreshOptions);

        return Ok(token);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<LoginResponseDto>> RefreshAsync(
    [FromBody] RefreshTokenRequestDto request,
    CancellationToken cancellationToken)
    {
        var result = await authService
            .LoginWithRefreshTokenAsync(request, cancellationToken);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(12),
            Path = "/",
            IsEssential = true
        };

        Response.Cookies.Append("jwt", result.AccessToken, cookieOptions);

        return Ok(result);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> LogoutAsync(CancellationToken cancellationToken)
    {
        await authService.LogoutAsync(cancellationToken);

        Response.Cookies.Delete("AccessToken");
        Response.Cookies.Delete("RefreshToken");

        return NoContent();
    }
}
