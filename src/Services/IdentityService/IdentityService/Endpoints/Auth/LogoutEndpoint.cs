using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Constants;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using IdentityService.Application.Features.Auth.Commands.Logout;
using MediatR;

namespace IdentityService.API.Endpoints.Auth;

public sealed class LogoutEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost($"{ApiConstants.Routes.Auth}/logout", async (
            ISender sender,
            IUserContext userContext,
            HttpContext context,
            CancellationToken cancellationToken) =>
        {
            var command = new LogoutCommand { UserId = userContext.UserId };

            Result result = await sender.Send(command, cancellationToken);

            CookieHelpers.ClearAuthCookies(context);

            return result.ToIResult();
        })
        .WithTags("Auth")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status401Unauthorized)
        .RequireAuthorization();
    }
}
