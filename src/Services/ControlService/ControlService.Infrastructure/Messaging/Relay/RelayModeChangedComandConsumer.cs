using Contracts.Events.RelayEvents;
using Control.Application.Interfaces;
using MassTransit;

namespace Control.Infrastructure.Messaging.Relay;

public class RelayModeChangedComandConsumer(IRelayServiceFromEvent service) 
    : IConsumer<ChangeRelayModeCommand>
{
    public async Task Consume(ConsumeContext<ChangeRelayModeCommand> context)
    {
        await service.ChangedModeFromEventAsync(
            context.Message, context.CancellationToken);
    }
}
