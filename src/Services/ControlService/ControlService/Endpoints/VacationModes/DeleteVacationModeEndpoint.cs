using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using Control.Application.Features.VacationModes.Commands.DeleteVacationMode;

namespace Control.API.Endpoints.VacationModes;

public sealed class DeleteVacationModeEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete($"{ApiConstants.Routes.VacationModes}/{{id:guid}}", async (
            Guid id,
            ISender sender,
            CancellationToken cancellationToken = default) =>
        {
            DeleteVacationModeCommand command = new DeleteVacationModeCommand { VacationModeId = id };

            var result = await sender.Send(command, cancellationToken);

            return result.ToIResult();
        })
        .WithTags("Vacation Modes")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status409Conflict)
        .RequireAuthorization(SubPermissions.VacationMode);
    }
}