using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Domain.Interfaces;

namespace Notification.Application.Services;

public class NotificationSender(
    IEnumerable<INotificationProvider> providers,
    IUserRepository userRepository,
    INotificationRepository notificationRepository,
    IUnitOfWork unitOfWork) : INotificationSender
{
    public async Task ProcessSingleNotificationAsync(
        NotificationEntity notification,
        CancellationToken cancellationToken)
    {
        var user = await userRepository
            .GetByIdAsync(notification.UserId, cancellationToken);

        bool overallSuccess = false;
        var errors = new List<string>();

        if (user == null || !user.IsNotifyEnabled)
        {
            return;
        }

        foreach (var provider in providers)
        {
            if (provider.IsEnabled(user))
            {
                var (success, error) = await provider
                    .SendAsync(user, notification.Message, cancellationToken);

                if (success)
                {
                    overallSuccess = true;
                }
                else
                {
                    errors.Add(error);
                }
            }
        }

        if (overallSuccess)
        {
            notification.MarkAsPublished();
        }
        else
        {
            notification.MarkAsFailure(string.Join(" | ", errors));
        }

        await notificationRepository.UpdateAsync(notification, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
