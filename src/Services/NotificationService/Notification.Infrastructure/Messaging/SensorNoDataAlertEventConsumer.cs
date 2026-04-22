using Contracts.Events.SensorEvents;
using MassTransit;
using Notification.Application.Interfaces;

namespace Notification.Infrastructure.Messaging;

public class SensorNoDataAlertEventConsumer(
    ISensorAlertSender alertSender) : IConsumer<SensorNoDataAlertEvent>
{
    public async Task Consume(ConsumeContext<SensorNoDataAlertEvent> context)
    {
        await alertSender.SendSensorNoDataAlertAsync(
            context.Message, context.CancellationToken);
    }
}
