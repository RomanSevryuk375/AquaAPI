using Notification.Domain.Entities;

namespace Notification.Application.Interfaces;

public interface INotificationSender
{
    Task ProcessSingleNotificationAsync(
        NotificationEntity notification, CancellationToken cancellationToken);
}