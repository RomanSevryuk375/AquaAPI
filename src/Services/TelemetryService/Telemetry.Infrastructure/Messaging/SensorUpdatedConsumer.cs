using Contracts.Events;
using MassTransit;
using Telemetry.Application.Interfaces;

namespace Telemetry.Infrastructure.Messaging;

public class SensorUpdatedConsumer(
    ISensorService sensorService) : IConsumer<SensorUpdatedEvent>
{
    public async Task Consume(ConsumeContext<SensorUpdatedEvent> context)
    {
        await sensorService.UpdateSensorFromEventAsync(
            context.Message, context.CancellationToken);
    }
}
