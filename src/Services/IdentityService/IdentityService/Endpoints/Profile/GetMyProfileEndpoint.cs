using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Authorization;
using BuildingBlocks.Presentation.Constants;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using IdentityService.Application.DTOs;
using IdentityService.Application.Features.Profile.Queries.GetMyProfile;
using MediatR;

namespace IdentityService.API.Endpoints.Profile;

public sealed class GetMyProfileEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiConstants.Routes.Profiles, async (
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new GetMyProfileQuery();
            Result<UserProfileResponseDto> result = await sender.Send(query, cancellationToken);

            return result.ToIResult();
        })
        .WithTags("Profile")
        .Produces<UserProfileResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .RequireAuthorization(SubPermissions.AccountView);
    }
}
