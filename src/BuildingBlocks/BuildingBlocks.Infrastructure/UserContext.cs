using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Presentation.Extensions;
using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Infrastructure;

public sealed class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    private static readonly Guid _systemUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    public Guid UserId
    {
        get
        {
            HttpContext? context = httpContextAccessor.HttpContext;
            return context is null
                ? _systemUserId
                : context.User.GetUserId();
        }
    }

    public bool IsAuthenticated => httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}
