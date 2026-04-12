using Contracts.Events.RelayEvents;
using Control.Application.Interfaces;
using MassTransit;

namespace Control.Infrastructure.Messaging.Relay;

public class RelayDeletedEventConsumer(IRelayServiceFromEvent service) 
    : IConsumer<RelayDeletedEvent>
{
    public async Task Consume(ConsumeContext<RelayDeletedEvent> context)
    {
        await service.DeletedRelayFromEventAsync(
            context.Message, context.CancellationToken);
    }
}
