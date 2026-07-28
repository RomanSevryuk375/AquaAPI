using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using Control.Application.Features.VacationModes.Commands.UpdateVacationMode;

namespace Control.API.Endpoints.VacationModes;

public sealed class UpdateVacationModeEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut($"{ApiConstants.Routes.VacationModes}/{{id:guid}}", async (
            Guid id,
            UpdateVacationModeCommand command,
            ISender sender,
            CancellationToken cancellationToken = default) =>
        {
            UpdateVacationModeCommand enrichedCommand = command with { VacationModeId = id };

            Result result = await sender.Send(enrichedCommand, cancellationToken);

            return result.ToIResult();
        })
        .WithTags("Vacation Modes")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status409Conflict)
        .RequireAuthorization(SubPermissions.VacationMode);
    }
}