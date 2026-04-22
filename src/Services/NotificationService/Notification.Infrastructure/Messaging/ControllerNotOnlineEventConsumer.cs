using Contracts.Events.ControllerEvents;
using MassTransit;
using Notification.Application.Interfaces;

namespace Notification.Infrastructure.Messaging;

public class ControllerNotOnlineEventConsumer(
    IControllerAlertSender alertSender) : IConsumer<ControllerNotOnlineEvent>
{
    public async Task Consume(ConsumeContext<ControllerNotOnlineEvent> context)
    {
        await alertSender.SendControllerNotOnlineAlert(
            context.Message, context.CancellationToken);
    }
}
