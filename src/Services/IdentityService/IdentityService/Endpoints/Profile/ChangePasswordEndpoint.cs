using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Authorization;
using BuildingBlocks.Presentation.Constants;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using IdentityService.Application.DTOs;
using IdentityService.Application.Features.Profile.Commands.ChangePassword;
using MediatR;

namespace IdentityService.API.Endpoints.Profile;

public sealed class ChangePasswordEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost($"{ApiConstants.Routes.Profiles}/password", async (
            ChangePasswordRequestDto request,
            ISender sender,
            IUserContext userContext,
            CancellationToken cancellationToken) =>
        {
            var command = new ChangePasswordCommand
            {
                UserId = userContext.UserId,
                CurrentPassword = request.CurrentPassword,
                NewPassword = request.NewPassword
            };

            Result result = await sender.Send(command, cancellationToken);

            return result.ToIResult();
        })
        .WithTags("Profile")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .RequireAuthorization(SubPermissions.AccountUpdate);
    }
}
