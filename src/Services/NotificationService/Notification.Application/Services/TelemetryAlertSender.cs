using Contracts.Enums;
using Contracts.Events.TelemetryEvents;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Domain.Interfaces;

namespace Notification.Application.Services;

public class TelemetryAlertSender(
    INotificationRepository notificationRepository,
    IUserRepository userRepository,
    IAquariumRepository aquariumRepository,
    IUnitOfWork unitOfWork) : ITelemetryAlertSender
{
    public async Task SendTelemetryAlertAsync(
        CriticalTelemetryThresholdAlertEvent alertEvent,
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
            $"In aquarium {existingAquarium.Name}," +
            $" sensor {alertEvent.SensorId} sent data {alertEvent.Value}, " +
            $"relay responsible for this sensor " +
            $"is in state {alertEvent.RelayState} at {alertEvent.RecordedAt:HH:mm:ss}");

        if (notification is null)
        {
            return;
        }

        await notificationRepository.AddAsync(notification, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
