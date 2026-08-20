using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using Control.Application.Features.Schedules.Queries.GetScheduleById;
using Control.Application.Features.Schedules.Queries.Shared;

namespace Control.API.Endpoints.Schedules;

public sealed class GetScheduleByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet($"{ApiConstants.Routes.Schedules}/{{id:guid}}", async (
            Guid id,
            ISender sender,
            IUserContext userContext,
            CancellationToken cancellationToken = default) =>
        {
            GetScheduleByIdQuery query = new GetScheduleByIdQuery
            {
                ScheduleId = id,
                UserId = userContext.UserId
            };

            Result<ScheduleDto> result = await sender.Send(query, cancellationToken);

            return result.ToIResult();
        })
        .WithName("GetScheduleById")
        .WithTags("Schedules")
        .Produces<ScheduleDto>()
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization(SubPermissions.AutoScheduleCreate);
    }
}