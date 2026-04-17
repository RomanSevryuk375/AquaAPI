using Notification.Application.Interfaces;
using Notification.Domain.Interfaces;

namespace Notification.Application.Services;

public class UnpublishedNoticeProcessor(
    INotificationRepository notificationRepository,
    INotificationSender sender) : IUnpublishedNoticeProcessor
{
    public async Task ProcessAsync(CancellationToken cancellationToken)
    {
        var notifications = await notificationRepository
            .GetUnpublishedNotificationsAsync(cancellationToken);

        if (notifications is null)
        {
            return;
        }

        foreach (var notification in notifications)
        {
            await sender
                .ProcessSingleNotificationAsync(notification, cancellationToken);
        }
    }
}
