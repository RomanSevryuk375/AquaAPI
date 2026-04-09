using Contracts.Events.SensorEvents;
using MassTransit;
using Telemetry.Application.Interfaces;

namespace Telemetry.Infrastructure.Messaging;

public class SensorDeletedConsumer(
    ISensorService sensorService) : IConsumer<SensorDeletedEvent>
{
    public async Task Consume(ConsumeContext<SensorDeletedEvent> context)
    {
        await sensorService.DeleteSensorFromEventAsync(
            context.Message, context.CancellationToken);
    }
}
