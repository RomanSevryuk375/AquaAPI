using Contracts.Events;
using MassTransit;
using System.Threading;
using Telemetry.Domain.Exceptions;
using Telemetry.Domain.Interfaces;

namespace Telemetry.Application.EventHandlers;

public class SensorDeletedConsumer(ISensorRepository sensorRepository, IUnitOfWork unitOfWork) : IConsumer<SensorDeletedEvent>
{
    public async Task Consume(ConsumeContext<SensorDeletedEvent> context)
    {
        var existingSensor = await sensorRepository.GetByIdAsync(context.Message.Id, context.CancellationToken);

        if (existingSensor is null)
        {
            return;
        }

        await sensorRepository.DeleteAsync(existingSensor.Id, context.CancellationToken);
        await unitOfWork.SaveChangesAsync(context.CancellationToken);
    }
}
