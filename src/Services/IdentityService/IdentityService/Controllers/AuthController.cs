using IdentityService.Application.DTOs;
using IdentityService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers;

[ApiController]
[Route("api/identity/v1/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<string>> RegisterAsync(
        [FromBody] RegisterUserRequestDto request,
        CancellationToken cancellationToken)
    {
        var token = await authService.RegisterUserAsync(request);

        return Ok(token);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> LoginAsync(
        [FromBody] LoginUserRequestDto request,
        CancellationToken cancellationToken)
    {
        var token = await authService.CheckLoginAsync(request);

        return Ok(token);
    }
}
