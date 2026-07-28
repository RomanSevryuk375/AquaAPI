using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Authorization;
using BuildingBlocks.Presentation.Constants;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using IdentityService.Application.DTOs;
using IdentityService.Application.Features.Profile.Commands.UpdateProfile;
using MediatR;

namespace IdentityService.API.Endpoints.Profile;

public sealed class UpdateMyProfileEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut($"{ApiConstants.Routes.Profiles}/me", async (
            UpdateProfileRequestDto request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateProfileCommand
            {
                Name = request.Name,
                PhoneNumber = request.PhoneNumber ?? string.Empty
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
