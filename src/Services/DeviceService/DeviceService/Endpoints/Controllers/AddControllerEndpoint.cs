using BuildingBlocks.Domain.Abstractions;
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
            IUserContext userContext,
            CancellationToken cancellationToken = default) =>
        {
            AddControllerCommand commandWithUser = command with { UserId = userContext.UserId, };
            Result<ControllerRegisteredResponse> result = await sender.Send(commandWithUser, cancellationToken);

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
