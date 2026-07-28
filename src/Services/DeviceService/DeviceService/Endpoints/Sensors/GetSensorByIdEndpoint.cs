using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Authorization;
using BuildingBlocks.Presentation.Constants;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using Device.Application.Features.Sensors.Query.GetSensorById;
using Device.Application.Features.Sensors.Query.Shared;
using MediatR;

namespace Device.API.Endpoints.Sensors;

public sealed class GetSensorByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet($"{ApiConstants.Routes.Sensors}/{{id:guid}}", async (
            Guid id,
            ISender sender,
            IUserContext userContext,
            CancellationToken cancellationToken = default) =>
        {
            var query = new GetSensorByIdQuery
            {
                UserId = userContext.UserId,
                SensorId = id
            };

            Result<SensorDto> result = await sender.Send(query, cancellationToken);

            return result.ToIResult();
        })
        .WithName("GetSensorById")
        .WithTags("Sensors")
        .Produces<SensorDto>()
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization(SubPermissions.DeviceControl);
    }
}
