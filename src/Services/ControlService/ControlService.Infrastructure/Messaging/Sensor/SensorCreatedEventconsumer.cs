using Contracts.Events.SensorEvents;
using Control.Application.Interfaces;
using MassTransit;

namespace Control.Infrastructure.Messaging.Sensor;

public class SensorCreatedEventconsumer(ISensorServiceFromEvent service) 
    : IConsumer<SensorCreatedEvent>
{
    public async Task Consume(ConsumeContext<SensorCreatedEvent> context)
    {
        await service.CreateSensorFromEventAsync(
            context.Message, context.CancellationToken);
    }
}
