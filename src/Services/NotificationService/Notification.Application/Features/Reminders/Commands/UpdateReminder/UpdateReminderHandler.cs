using BuildingBlocks.Domain.Results;
using MediatR;
using Notification.Application.Constants;
using Notification.Domain.Entities;
using Notification.Domain.Interfaces;
using ZiggyCreatures.Caching.Fusion;

namespace Notification.Application.Features.Reminders.Commands.UpdateReminder;

public sealed class UpdateReminderHandler(
    IReminderRepository reminderRepository,
    IFusionCache cache)
    : IRequestHandler<UpdateReminderCommand, Result>
{
    public async Task<Result> Handle(UpdateReminderCommand request, CancellationToken cancellationToken)
    {
        Reminder? reminder = await reminderRepository.GetByIdAsync(request.ReminderId, cancellationToken);

        Result updateResult = reminder!.UpdateSchedule(request.TaskName, request.IntervalDays);
        if (updateResult.IsFailure)
        {
            return Result.Failure(updateResult.Error);
        }

        await cache.RemoveAsync(CacheKeys.Reminder(reminder.UserId, reminder.Id), token: cancellationToken);

        return Result.Success();
    }
}
