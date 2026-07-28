using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Authorization;
using BuildingBlocks.Presentation.Constants;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using Device.Application.Features.Controllers.Query.GetControllerById;
using Device.Application.Features.Controllers.Query.Shared;
using MediatR;

namespace Device.API.Endpoints.Controllers;

public sealed class GetControllerByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet($"{ApiConstants.Routes.Controllers}/{{id:guid}}", async (
            Guid id,
            ISender sender,
            IUserContext userContext,
            CancellationToken cancellationToken = default) =>
        {
            var query = new GetControllerByIdQuery
            {
                UserId = userContext.UserId,
                ControllerId = id
            };

            Result<ControllerDto> result = await sender.Send(query, cancellationToken);

            return result.ToIResult();
        })
        .WithName("GetControllerById")
        .WithTags("Controllers")
        .Produces<ControllerDto>()
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization(SubPermissions.DeviceControl);
    }
}
