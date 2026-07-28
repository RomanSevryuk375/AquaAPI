using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Authorization;
using BuildingBlocks.Presentation.Constants;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using Device.Application.Features.Controllers.Command.AddController;
using MediatR;

namespace Device.API.Endpoints.Controllers;

public sealed class AddControllerEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiConstants.Routes.Controllers, async (
            AddControllerCommand command,
            ISender sender,
            CancellationToken cancellationToken = default) =>
        {
            Result<ControllerRegisteredResponse> result = await sender.Send(command, cancellationToken);

            return result.IsSuccess
                ? Results.CreatedAtRoute("GetControllerById", new { id = result.Value.ControllerId }, result.Value)
                : result.ToIResult();
        })
        .WithTags("Controllers")
        .Produces<ControllerRegisteredResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization(SubPermissions.DeviceControl);
    }
}
