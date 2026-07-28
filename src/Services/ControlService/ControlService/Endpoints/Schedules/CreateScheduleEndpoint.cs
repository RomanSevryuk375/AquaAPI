using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using Control.Application.Features.Schedules.Commands.CreateSchedule;

namespace Control.API.Endpoints.Schedules;

public sealed class CreateScheduleEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiConstants.Routes.Schedules, async (
            CreateScheduleCommand command,
            ISender sender,
            CancellationToken cancellationToken = default) =>
        {
            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.IsSuccess
                ? Results.CreatedAtRoute("GetScheduleById", new { id = result.Value }, result.Value)
                : result.ToIResult();
        })
        .WithTags("Schedules")
        .Produces<Guid>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict)
        .RequireAuthorization(SubPermissions.AutoScheduleCreate);
    }
}