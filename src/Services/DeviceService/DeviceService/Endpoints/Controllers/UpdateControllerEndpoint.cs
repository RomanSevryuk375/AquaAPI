using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Authorization;
using BuildingBlocks.Presentation.Constants;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using Device.Application.Features.Controllers.Command.UpdateController;
using MediatR;

namespace Device.API.Endpoints.Controllers;

public sealed class UpdateControllerEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut($"{ApiConstants.Routes.Controllers}/{{id:guid}}", async (
            Guid id,
            UpdateControllerCommand command,
            ISender sender,
            CancellationToken cancellationToken = default) =>
        {
            UpdateControllerCommand enrichedCommand = command with { ControllerId = id };

            Result result = await sender.Send(enrichedCommand, cancellationToken);

            return result.ToIResult();
        })
        .WithTags("Controllers")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization(SubPermissions.DeviceControl);
    }
}
