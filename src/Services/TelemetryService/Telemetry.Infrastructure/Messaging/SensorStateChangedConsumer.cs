using Contracts.Events.SensorEvents;
using MassTransit;
using Telemetry.Application.Interfaces;

namespace Telemetry.Infrastructure.Messaging;

public class SensorStateChangedConsumer(
    ISensorService sensorService) : IConsumer<SensorStateChangedCommand>
{
    public async Task Consume(ConsumeContext<SensorStateChangedCommand> context)
    {
        await sensorService
            .SetSensorStateFromEventAsync(
                context.Message, 
                context.CancellationToken);
    }
}
