using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using Device.Application.Constants;
using Device.Application.Features.Sensors.Query.Shared;

namespace Device.Application.Features.Sensors.Query.GetSensorById;

public sealed record GetSensorByIdQuery
    : ICachedQuery<Result<SensorDto>>
{
    public Guid UserId { get; init; }
    public Guid SensorId { get; init; }

    public string CacheKey => CacheKeys.Sensor(UserId, SensorId);
    public TimeSpan? Expiration => TimeSpan.FromMinutes(5);
}
