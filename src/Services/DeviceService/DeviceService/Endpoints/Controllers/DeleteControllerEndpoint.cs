using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Authorization;
using BuildingBlocks.Presentation.Constants;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using Device.Application.Features.Controllers.Command.DeleteController;
using MediatR;

namespace Device.API.Endpoints.Controllers;

public sealed class DeleteControllerEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete($"{ApiConstants.Routes.Controllers}/{{id:guid}}", async (
            Guid id,
            ISender sender,
            IUserContext userContext,
            CancellationToken cancellationToken = default) =>
        {
            var command = new DeleteControllerCommand 
            { 
                ControllerId = id,
                UserId = userContext.UserId 
            };

            Result result = await sender.Send(command, cancellationToken);

            return result.ToIResult();
        })
        .WithTags("Controllers")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization(SubPermissions.DeviceControl);
    }
}
