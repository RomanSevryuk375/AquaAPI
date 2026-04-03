using Contracts.Enums;
using Contracts.Events;
using MassTransit;
using Telemetry.Domain.Exceptions;
using Telemetry.Domain.Interfaces;

namespace Telemetry.Application.EventHandlers;

public class SensorUpdatedConsumer(
    ISensorRepository sensorRepository, 
    IUnitOfWork unitOfWork) : IConsumer<SensorUpdatedEvent>
{
    public async Task Consume(ConsumeContext<SensorUpdatedEvent> context)
    {
        var existingSensor = await sensorRepository.GetByIdAsync(context.Message.Id, context.CancellationToken)
            ?? throw new NotFoundException($"Sensor {context.Message.Id} not found");

        var errors = existingSensor.Update(
            context.Message.ControllerId,
            context.Message.Type,
            context.Message.Unit,
            context.Message.LastValue,
            context.Message.UpdatedAt);

        if (errors is not null && errors.Count > 0)
        {
            throw new DomainValidationException(
                $"Update failed: {string.Join(", ", errors)}");
        }

        await sensorRepository.UpdateAsync(existingSensor, context.CancellationToken);
        await unitOfWork.SaveChangesAsync(context.CancellationToken);
    }
}
