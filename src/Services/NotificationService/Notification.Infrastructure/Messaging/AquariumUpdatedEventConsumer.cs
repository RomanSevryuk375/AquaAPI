using Contracts.Events.AquariumEvents;
using MassTransit;
using Notification.Application.Interfaces;

namespace Notification.Infrastructure.Messaging;

public class AquariumUpdatedEventConsumer(
    IAquariumServiceFromEvent service) : IConsumer<AquarimUdatedEvent>
{
    public async Task Consume(ConsumeContext<AquarimUdatedEvent> context)
    {
        await service.UpdateAquariumFromEventAsync(
            context.Message, context.CancellationToken);
    }
}
