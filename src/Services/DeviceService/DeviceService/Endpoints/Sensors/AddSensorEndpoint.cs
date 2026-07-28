using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Authorization;
using BuildingBlocks.Presentation.Constants;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using Device.Application.Features.Sensors.Command.AddSensor;
using MediatR;

namespace Device.API.Endpoints.Sensors;

public sealed class AddSensorEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiConstants.Routes.Sensors, async (
            AddSensorCommand command,
            ISender sender,
            CancellationToken cancellationToken = default) =>
        {
            Result<SensorCreatedResponse> result = await sender.Send(command, cancellationToken);

            return result.IsSuccess
                ? Results.CreatedAtRoute("GetSensorById", new { id = result.Value.Id }, result.Value)
                : result.ToIResult();
        })
        .WithTags("Sensors")
        .Produces<SensorCreatedResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization(SubPermissions.DeviceControl);
    }
}
