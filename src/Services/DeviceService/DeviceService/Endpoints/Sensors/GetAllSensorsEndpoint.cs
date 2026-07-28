using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Enums;
using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Authorization;
using BuildingBlocks.Presentation.Constants;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using Device.Application.Features.Sensors.Query.GetAllSensors;
using Device.Application.Features.Sensors.Query.Shared;
using MediatR;

namespace Device.API.Endpoints.Sensors;

public sealed class GetAllSensorsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiConstants.Routes.Sensors, async (
            Guid? controllerId,
            SensorType? type,
            SensorState? state,
            ISender sender,
            IUserContext userContext,
            int skip = 0,
            int take = 10,
            CancellationToken cancellationToken = default) =>
        {
            var query = new GetAllSensorsQuery
            {
                UserId = userContext.UserId,
                ControllerId = controllerId,
                Type = type,
                State = state,
                Skip = skip,
                Take = take
            };

            Result<IReadOnlyList<SensorDto>> result = await sender.Send(query, cancellationToken);

            return result.ToIResult();
        })
        .WithTags("Sensors")
        .Produces<IReadOnlyList<SensorDto>>()
        .RequireAuthorization(SubPermissions.DeviceControl);
    }
}
