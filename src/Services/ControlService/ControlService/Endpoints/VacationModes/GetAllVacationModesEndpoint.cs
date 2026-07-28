using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using Control.Application.Features.VacationModes.Queries.GetAllVacationModes;
using Control.Application.Features.VacationModes.Queries.Shared;

namespace Control.API.Endpoints.VacationModes;

public sealed class GetAllVacationModesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiConstants.Routes.VacationModes, async (
            Guid ecosystemId,
            bool? isActive,
            ISender sender,
            IUserContext userContext,
            int skip = 0,
            int take = 10,
            CancellationToken cancellationToken = default) =>
        {
            GetAllVacationModesQuery query = new GetAllVacationModesQuery
            {
                UserId = userContext.UserId,
                EcosystemId = ecosystemId,
                IsActive = isActive,
                Skip = skip,
                Take = take
            };

            var result = await sender.Send(query, cancellationToken);

            return result.ToIResult();
        })
        .WithTags("Vacation Modes")
        .Produces<IReadOnlyList<VacationModeDto>>()
        .RequireAuthorization(SubPermissions.VacationMode);
    }
}