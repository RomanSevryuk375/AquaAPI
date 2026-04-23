using Contracts.Events.AquariumEvents;
using MassTransit;
using Notification.Application.Interfaces;

namespace Notification.Infrastructure.Messaging;

public class AquariumDeletedEventConsumer(
    IAquariumServiceFromEvent service) : IConsumer<AquariumDeletedEvent>
{
    public async Task Consume(ConsumeContext<AquariumDeletedEvent> context)
    {
        await service.DeleteAquariumFromEventAsync(
            context.Message, context.CancellationToken);
    }
}
