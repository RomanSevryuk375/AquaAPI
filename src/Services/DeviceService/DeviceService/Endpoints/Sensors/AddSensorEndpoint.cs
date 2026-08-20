using BuildingBlocks.Domain.Abstractions;
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
            IUserContext userContext,
            CancellationToken cancellationToken = default) =>
        {
            AddSensorCommand commandWithUser = command with { UserId = userContext.UserId };
            Result<SensorCreatedResponse> result = await sender.Send(commandWithUser, cancellationToken);

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
