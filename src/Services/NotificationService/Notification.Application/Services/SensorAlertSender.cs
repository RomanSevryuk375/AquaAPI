using Contracts.Enums;
using Contracts.Events.SensorEvents;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Domain.Interfaces;

namespace Notification.Application.Services;

public class SensorAlertSender(
    INotificationRepository notificationRepository,
    IUserRepository userRepository,
    IAquariumRepository aquariumRepository,
    IUnitOfWork unitOfWork) : ISensorAlertSender
{
    public async Task SendSensorNoDataAlertAsync(
        SensorNoDataAlertEvent alertEvent,
        CancellationToken cancellationToken)
    {
        var existingUser = await userRepository
            .ExistsAsync(alertEvent.UserId, cancellationToken);

        if (!existingUser)
        {
            return;
        }

        var existingAquarium = await aquariumRepository
            .GetByIdAsync(alertEvent.AquariumId, cancellationToken);

        if (existingAquarium is null)
        {
            return;
        }

        var (notification, errors) = NotificationEntity.Create(
            alertEvent.UserId,
            alertEvent.AquariumId,
            NotificationLevelEnum.Critical,
            $"Sensor {alertEvent.SensorId} " +
            $"from aquarium {existingAquarium.Name} did not send data " +
            $"at time {alertEvent.LastSeenAt:HH:mm:ss}");

        if (notification is null)
        {
            return;
        }

        await notificationRepository.AddAsync(notification, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
