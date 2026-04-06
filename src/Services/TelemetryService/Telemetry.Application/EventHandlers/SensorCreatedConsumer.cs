using Contracts.Events;
using MassTransit;
using Telemetry.Domain.Entities;
using Telemetry.Domain.Exceptions;
using Telemetry.Domain.Interfaces;

namespace Telemetry.Application.EventHandlers;

public class SensorCreatedConsumer(
    ISensorRepository sensorRepository,
    IUnitOfWork unitOfWork) : IConsumer<SensorCreatedEvent>
{
    public async Task Consume(ConsumeContext<SensorCreatedEvent> context)
    {
        var exists = await sensorRepository.ExistsAsync(context.Message.Id, context.CancellationToken);
        if (exists)
        {
            return;
        }

        var (sensor, errors) = SensorEntity.Create(
            context.Message.Id,
            context.Message.ControllerId,
            context.Message.Type,
            context.Message.Unit,
            context.Message.LastValue,
            context.Message.UpdatedAt,
            context.Message.CreatedAt);

        if (sensor is null)
        {
            throw new DomainValidationException(
                $"Failed to create {nameof(SensorEntity)}: {string.Join(", ", errors)}");
        }

        var result = await sensorRepository.AddAsync(sensor, context.CancellationToken);
        await unitOfWork.SaveChangesAsync(context.CancellationToken); 
    }
}
