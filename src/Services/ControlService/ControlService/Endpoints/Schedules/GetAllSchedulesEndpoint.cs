using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using Control.Application.Features.Schedules.Queries.GetAllSchedules;
using Control.Application.Features.Schedules.Queries.Shared;

namespace Control.API.Endpoints.Schedules;

public sealed class GetAllSchedulesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiConstants.Routes.Schedules, async (
            Guid ecosystemId,
            Guid? relayId,
            bool? isEnabled,
            ISender sender,
            IUserContext userContext,
            int skip = 0,
            int take = 10,
            CancellationToken cancellationToken = default) =>
        {
            GetAllSchedulesQuery query = new GetAllSchedulesQuery
            {
                UserId = userContext.UserId,
                EcosystemId = ecosystemId,
                RelayId = relayId,
                IsEnabled = isEnabled,
                Skip = skip,
                Take = take
            };

            var result = await sender.Send(query, cancellationToken);

            return result.ToIResult();
        })
        .WithTags("Schedules")
        .Produces<IReadOnlyList<ScheduleDto>>()
        .RequireAuthorization(SubPermissions.AutoScheduleCreate);
    }
}