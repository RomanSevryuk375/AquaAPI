using BuildingBlocks.Domain.Results;
using Control.Application.Constants;
using Control.Domain.Entities;
using Control.Domain.Interfaces;
using MediatR;
using ZiggyCreatures.Caching.Fusion;

namespace Control.Application.Features.Schedules.Commands.SetIsActiveSchedule;

public sealed class SetIsActiveScheduleHandler(
    IScheduleRepository scheduleRepository,
    IFusionCache cache)
    : IRequestHandler<SetIsActiveScheduleCommand, Result>
{
    public async Task<Result> Handle(SetIsActiveScheduleCommand request, CancellationToken cancellationToken)
    {
        Schedule? schedule = await scheduleRepository.GetByIdAsync(request.ScheduleId, cancellationToken);
        if (schedule is null)
        {
            return Result.Failure(Error.NotFound<Schedule>(
                $"Schedule {request.ScheduleId}not found."));
        }

        schedule.SetIsActive(request.IsActive);

        await cache.RemoveAsync(CacheKeys.Schedule(request.UserId, request.ScheduleId), token: cancellationToken);

        return Result.Success();
    }
}
