using Contracts.Enums;
using Contracts.Events.ControllerEvents;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Domain.Interfaces;

namespace Notification.Application.Services;

public class ControllerAlertSender(
    INotificationRepository notificationRepository,
    IUserRepository userRepository,
    IAquariumRepository aquariumRepository,
    IUnitOfWork unitOfWork) : IControllerAlertSender
{
    public async Task SendControllerNotOnlineAlert(
        ControllerNotOnlineEvent controllerEvent,
        CancellationToken cancellationToken)
    {
        var existingUser = await userRepository
            .GetByIdAsync(controllerEvent.UserId, cancellationToken);

        if (existingUser is null)
        {
            return;
        }

        var existingAquarium = await aquariumRepository
            .GetByUserIdAsync(existingUser.Id, cancellationToken);

        if (existingAquarium is null)
        {
            return;
        }

        var (notification, errors) = NotificationEntity.Create(
            controllerEvent.UserId,
            existingAquarium.Id,
            NotificationLevelEnum.Critical,
            $"Controller {controllerEvent.ControllerId} " +
            $"was last online at {controllerEvent.LastSeenAt:HH:mm:ss}");

        if (notification is null)
        {
            return;
        }

        await notificationRepository.AddAsync(notification, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
