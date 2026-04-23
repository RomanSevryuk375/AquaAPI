using Contracts.Events.TelemetryEvents;

namespace Notification.Application.Interfaces;

public interface ITelemetryAlertSender
{
    Task SendTelemetryAlertAsync(
        CriticalTelemetryThresholdAlertEvent alertEvent, 
        CancellationToken cancellationToken);
}