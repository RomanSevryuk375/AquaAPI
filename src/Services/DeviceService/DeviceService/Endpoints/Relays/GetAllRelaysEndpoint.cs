using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Enums;
using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Authorization;
using BuildingBlocks.Presentation.Constants;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using Device.Application.Features.Relays.Query.GetAllRelays;
using Device.Application.Features.Relays.Query.Shared;
using MediatR;

namespace Device.API.Endpoints.Relays;

public sealed class GetAllRelaysEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiConstants.Routes.Relays, async (
            Guid? controllerId,
            RelayPurpose? purpose,
            bool? isActive,
            bool? isManual,
            ISender sender,
            IUserContext userContext,
            int skip = 0,
            int take = 10,
            CancellationToken cancellationToken = default) =>
        {
            var query = new GetAllRelaysQuery
            {
                UserId = userContext.UserId,
                ControllerId = controllerId,
                Purpose = purpose,
                IsActive = isActive,
                IsManual = isManual,
                Skip = skip,
                Take = take
            };

            Result<IReadOnlyList<RelayDto>> result = await sender.Send(query, cancellationToken);

            return result.ToIResult();
        })
        .WithTags("Relays")
        .Produces<IReadOnlyList<RelayDto>>()
        .RequireAuthorization(SubPermissions.DeviceControl);
    }
}
