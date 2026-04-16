using Contracts.Events.Aquariums;
using MassTransit;
using Notification.Application.Interfaces;

namespace Notification.Infrastructure.Messaging;

public class AquariumCreatedEventConsumer(
    IAquariumServiceFromEvent service) : IConsumer<AquariumCreatedEvend>
{
    public async Task Consume(ConsumeContext<AquariumCreatedEvend> context)
    {
        await service.CreateAquariumFromEventAsync(
            context.Message, context.CancellationToken);
    }
}
