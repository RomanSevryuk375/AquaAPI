using BuildingBlocks.Domain.Results;
using Control.Application.Constants;
using Control.Domain.Interfaces;
using MediatR;
using ZiggyCreatures.Caching.Fusion;

namespace Control.Application.Features.Schedules.Commands.DeleteSchedule;

public sealed class DeleteScheduleHandler(
    IScheduleRepository scheduleRepository,
    IFusionCache cache)
    : IRequestHandler<DeleteScheduleCommand, Result>
{
    public async Task<Result> Handle(DeleteScheduleCommand request, CancellationToken cancellationToken)
    {
        await scheduleRepository.DeleteAsync(request.ScheduleId, cancellationToken);

        await cache.RemoveAsync(CacheKeys.Schedule(request.UserId, request.ScheduleId), token: cancellationToken);

        return Result.Success();
    }
}
