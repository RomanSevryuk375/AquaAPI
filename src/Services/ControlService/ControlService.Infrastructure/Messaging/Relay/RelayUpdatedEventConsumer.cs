using Contracts.Events.RelayEvents;
using Control.Application.Interfaces;
using MassTransit;

namespace Control.Infrastructure.Messaging.Relay;

public class RelayUpdatedEventConsumer(IRelayServiceFromEvent service)
    : IConsumer<RelayUpdatedEvent>
{
    public async Task Consume(ConsumeContext<RelayUpdatedEvent> context)
    {
        await service.UpdatedRelayFromEventAsync(
            context.Message, context.CancellationToken);
    }
}
