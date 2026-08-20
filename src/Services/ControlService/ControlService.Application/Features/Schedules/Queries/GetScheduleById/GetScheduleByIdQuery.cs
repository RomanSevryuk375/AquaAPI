using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using Control.Application.Constants;
using Control.Application.Features.Schedules.Queries.Shared;
using Control.Application.Interfaces;

namespace Control.Application.Features.Schedules.Queries.GetScheduleById;

public sealed record GetScheduleByIdQuery
    : ICachedQuery<Result<ScheduleDto>>, IScheduleBoundRequest
{
    public Guid ScheduleId { get; init; }
    public Guid UserId { get; init; }

    public string CacheKey => CacheKeys.Schedule(UserId, ScheduleId);
    public TimeSpan? Expiration => TimeSpan.FromMinutes(2);
}
