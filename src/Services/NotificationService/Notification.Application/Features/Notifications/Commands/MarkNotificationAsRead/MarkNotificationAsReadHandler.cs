using BuildingBlocks.Domain.Results;
using MediatR;
using Notification.Application.Constants;
using Notification.Domain.Interfaces;
using ZiggyCreatures.Caching.Fusion;

namespace Notification.Application.Features.Notifications.Commands.MarkNotificationAsRead;

public sealed class MarkNotificationAsReadHandler(
    INotificationRepository notificationRepository,
    IFusionCache cache) : IRequestHandler<MarkNotificationAsReadCommand, Result>
{
    public async Task<Result> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Notification? notification = await notificationRepository.GetByIdAsync(
            request.NotificationId, cancellationToken);
        if (notification is null || notification.UserId != request.UserId)
        {
            return Result.Failure(Error.NotFound<Domain.Entities.Notification>(
                $"Notification {request.NotificationId} not found."));
        }

        if (notification.IsRead)
        {
            return Result.Success();
        }

        notification.MarkAsRead();

        await cache.RemoveAsync(CacheKeys.Notification(notification.UserId, notification.Id), token: cancellationToken);

        return Result.Success();
    }
}
