using BuildingBlocks.Domain.Abstractions;

using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Authorization;
using BuildingBlocks.Presentation.Constants;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using Device.Application.Features.Relays.Query.GetRelayById;
using Device.Application.Features.Relays.Query.Shared;
using MediatR;

namespace Device.API.Endpoints.Relays;

public sealed class GetRelayByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet($"{ApiConstants.Routes.Relays}/{{id:guid}}", async (
            Guid id,
            ISender sender,
            IUserContext userContext,
            CancellationToken cancellationToken = default) =>
        {
            var query = new GetRelayByIdQuery
            {
                UserId = userContext.UserId,
                RelayId = id
            };

            Result<RelayDto> result = await sender.Send(query, cancellationToken);

            return result.ToIResult();
        })
        .WithName("GetRelayById")
        .WithTags("Relays")
        .Produces<RelayDto>()
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization(SubPermissions.DeviceControl);
    }
}
