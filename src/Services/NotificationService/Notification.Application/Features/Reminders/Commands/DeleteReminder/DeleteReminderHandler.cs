using BuildingBlocks.Domain.Results;
using MediatR;
using Notification.Application.Constants;
using Notification.Domain.Interfaces;
using ZiggyCreatures.Caching.Fusion;

namespace Notification.Application.Features.Reminders.Commands.DeleteReminder;

public sealed class DeleteReminderHandler(
    IReminderRepository reminderRepository,
    IFusionCache cache) : IRequestHandler<DeleteReminderCommand, Result>
{
    public async Task<Result> Handle(DeleteReminderCommand request, CancellationToken cancellationToken)
    {
        await reminderRepository.DeleteAsync(request.ReminderId, cancellationToken);

        await cache.RemoveAsync(CacheKeys.Reminder(request.UserId, request.ReminderId), token: cancellationToken);

        return Result.Success();
    }
}
