using Contracts.Events.SensorEvents;

namespace Notification.Application.Interfaces;

public interface ISensorAlertSender
{
    Task SendSensorNoDataAlertAsync(
        SensorNoDataAlertEvent alertEvent, 
        CancellationToken cancellationToken);
}