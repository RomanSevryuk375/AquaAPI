using Contracts.Events.SensorEvents;
using MassTransit;
using Telemetry.Application.Interfaces;

namespace Telemetry.Infrastructure.Messaging;

public class SensorCreatedConsumer(
    ISensorService sensorService) : IConsumer<SensorCreatedEvent>
{
    public async Task Consume(ConsumeContext<SensorCreatedEvent> context)
    {
        await sensorService.CreateSensorFromEventAsync(
            context.Message, context.CancellationToken);
    }
}
