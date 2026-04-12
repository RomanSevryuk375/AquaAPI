using Control.Application.Interfaces;
using MassTransit;

namespace Control.Infrastructure.Messaging.Relay;

public class RelayModeChangedComandConsumer(IRelayServiceFromEvent service) 
    : IConsumer<RelayModeChangedCommand>
{
    public async Task Consume(ConsumeContext<RelayModeChangedCommand> context)
    {
        await service.ChangedModeFromEventAsync(
            context.Message, context.CancellationToken);
    }
}
