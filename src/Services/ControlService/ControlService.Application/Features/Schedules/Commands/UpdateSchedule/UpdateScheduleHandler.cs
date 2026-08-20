using BuildingBlocks.Domain.Results;
using Control.Application.Constants;
using Control.Domain.Entities;
using Control.Domain.Interfaces;
using MediatR;
using ZiggyCreatures.Caching.Fusion;

namespace Control.Application.Features.Schedules.Commands.UpdateSchedule;

public sealed class UpdateScheduleHandler(
    IScheduleRepository scheduleRepository,
    ICronValidator cronValidator,
    IFusionCache cache) : IRequestHandler<UpdateScheduleCommand, Result>
{
    public async Task<Result> Handle(UpdateScheduleCommand request, CancellationToken cancellationToken)
    {
        Schedule? schedule = await scheduleRepository.GetByIdAsync(request.ScheduleId, cancellationToken);
        if (schedule is null)
        {
            return Result.Failure(Error.NotFound<Schedule>(
                $"Schedule {request.ScheduleId}not found."));
        }

        Result updateResult = schedule.Update(
            request.CronExpression, cronValidator, request.DurationMin,
            request.IsFadeMode, request.IsEnabled);
        if (updateResult.IsFailure)
        {
            return Result.Failure(updateResult.Error);
        }

        await cache.RemoveAsync(CacheKeys.Schedule(request.UserId, request.ScheduleId), token: cancellationToken);

        return Result.Success();
    }
}
