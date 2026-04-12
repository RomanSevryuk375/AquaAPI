using Contracts.Events.RelayEvents;
using Device.Application.Interfaces;
using MassTransit;

namespace Device.Infrastructure.Messaging;

public class RelayChangeStateConsumer(IRelayService service) 
    : IConsumer<ChangeRelayStateCommand>
{
    public async Task Consume(ConsumeContext<ChangeRelayStateCommand> context)
    {
        await service.SetRelayStateFromCommandAsync(
            context.Message, context.CancellationToken);
    }
}