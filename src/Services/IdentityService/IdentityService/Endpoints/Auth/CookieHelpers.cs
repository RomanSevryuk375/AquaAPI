using BuildingBlocks.Presentation.Constants;
using IdentityService.Application.DTOs;

namespace IdentityService.API.Endpoints.Auth;

internal static class CookieHelpers
{
    public static void AppendAuthCookies(HttpContext context, LoginResponseDto token)
    {
        context.Response.Cookies.Append(
            AuthConstants.AccessTokenCookieName,
            token.AccessToken,
            CreateAccessTokenCookieOptions());

        context.Response.Cookies.Append(
            AuthConstants.RefreshTokenCookieName,
            token.RefreshToken,
            CreateRefreshTokenCookieOptions());
    }

    public static void ClearAuthCookies(HttpContext context)
    {
        context.Response.Cookies.Delete(
            AuthConstants.AccessTokenCookieName,
            CreateAccessTokenCookieOptions());

        context.Response.Cookies.Delete(
            AuthConstants.RefreshTokenCookieName,
            CreateRefreshTokenCookieOptions());
    }

    private static CookieOptions CreateAccessTokenCookieOptions() => new()
    {
        HttpOnly = true,
        Secure = false,
        SameSite = SameSiteMode.Strict,
        Expires = DateTimeOffset.UtcNow.AddHours(12),
        Path = "/",
        IsEssential = true
    };

    private static CookieOptions CreateRefreshTokenCookieOptions() => new()
    {
        HttpOnly = true,
        Secure = false,
        SameSite = SameSiteMode.Strict,
        Expires = DateTimeOffset.UtcNow.AddDays(30),
        Path = "/api/identity/v1/auth",
        IsEssential = true
    };
}
