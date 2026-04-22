using Contracts.Events.TelemetryEvents;
using MassTransit;
using Notification.Application.Interfaces;

namespace Notification.Infrastructure.Messaging;

internal class CriticalTelemetryThresholdAlertEventConsumer(
    ITelemetryAlertSender alertSender) : IConsumer<CriticalTelemetryThresholdAlertEvent>
{
    public async Task Consume(ConsumeContext<CriticalTelemetryThresholdAlertEvent> context)
    {
        await alertSender.SendTelemetryAlertAsync(
            context.Message, context.CancellationToken)
    }
}
