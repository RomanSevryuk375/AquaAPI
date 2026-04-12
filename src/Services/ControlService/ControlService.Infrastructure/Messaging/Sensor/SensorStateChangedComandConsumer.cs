using Contracts.Events.SensorEvents;
using Control.Application.Interfaces;
using MassTransit;

namespace Control.Infrastructure.Messaging.Sensor;

public class SensorStateChangedComandConsumer(
    ISensorServiceFromEvent service) : IConsumer<SensorStateChangedCommand>
{
    public async Task Consume(ConsumeContext<SensorStateChangedCommand> context)
    {
        await service.ChangedStateFromEventAsync(
            context.Message, context.CancellationToken);
    }
}
