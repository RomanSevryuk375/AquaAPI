using Notification.Domain.Entities;

namespace Notification.Domain.Interfaces;

public interface INotificationProvider
{
    bool IsEnabled(UserEntity user);
    Task<(bool Success, string Error)> SendAsync(
        UserEntity user, string message, CancellationToken cancellationToken);
}
