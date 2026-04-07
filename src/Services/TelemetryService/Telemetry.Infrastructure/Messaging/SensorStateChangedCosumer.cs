using Contracts.Events.SensorEvents;
using MassTransit;
using Telemetry.Application.Interfaces;

namespace Telemetry.Infrastructure.Messaging;

public class SensorStateChangedCosumer(
    ISensorService sensorService) : IConsumer<SensorStateChangedEvent>
{
    public async Task Consume(ConsumeContext<SensorStateChangedEvent> context)
    {
        await sensorService
            .SetSensorStateFromEventAsync(
                context.Message, 
                context.CancellationToken);
    }
}
