using Contracts.Enums;
using Contracts.Events.SensorEvents;
using Telemetry.Application.Interfaces;
using Telemetry.Domain.Entities;
using Telemetry.Domain.Interfaces;

namespace Telemetry.Application.Services;

public class SensorService(
    ISensorRepository sensorRepository,
    IUnitOfWork unitOfWork) : ISensorService
{
    public async Task CreateSensorFromEventAsync(
        SensorCreatedEvent sensorCreated, 
        CancellationToken cancellationToken)
    {
        var exists = await sensorRepository
            .ExistsAsync(sensorCreated.Id, cancellationToken);

        if (exists)
        {
            return;
        }

        var (sensor, errors) = SensorEntity.Create(
            sensorCreated.Id,
            sensorCreated.ControllerId,
            sensorCreated.Type,
            sensorCreated.State,
            sensorCreated.Unit,
            0.0,
            DateTime.UtcNow,
            sensorCreated.CreatedAt);

        if (sensor is null)
        {
            return;
        }

        await sensorRepository.AddAsync(sensor, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteSensorFromEventAsync(
        SensorDeletedEvent sensorDeleted, 
        CancellationToken cancellationToken)
    {
        var existingSensor = await sensorRepository
            .GetByIdAsync(sensorDeleted.Id, cancellationToken);

        if (existingSensor is null)
        {
            return;
        }

        await sensorRepository.DeleteAsync(existingSensor.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateSensorFromEventAsync(
        SensorUpdatedEvent updatedEvent, 
        CancellationToken cancellationToken)
    {
        var existingSensor = await sensorRepository
            .GetByIdAsync(updatedEvent.Id, cancellationToken);

        if (existingSensor is null)
        {
            var (sensor, createErrors) = SensorEntity.Create(
            updatedEvent.Id,
            updatedEvent.ControllerId,
            updatedEvent.Type,
            SensorStateEnum.NoData,
            updatedEvent.Unit,
            updatedEvent.LastValue,
            updatedEvent.UpdatedAt,
            updatedEvent.CreatedAt);

            if (sensor is null)
            {
                return;
            }

            await sensorRepository.AddAsync(sensor, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return;
        }

        var errors = existingSensor!.Update(
            updatedEvent.ControllerId,
            updatedEvent.Type,
            updatedEvent.Unit);

        if (errors is not null && errors.Count > 0)
        {
            return;
        }

        await sensorRepository.UpdateAsync(existingSensor, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task SetSensorStateFromEventAsync(
        SensorStateChangedEvent sensorStateChanged,
        CancellationToken cancellationToken)
    {
        var existingSensor = await sensorRepository
            .GetByIdAsync(sensorStateChanged.Id, cancellationToken);

        if (existingSensor is null)
        {
            return;
        }

        existingSensor.SetState(sensorStateChanged.State);

        await sensorRepository.UpdateAsync(existingSensor, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
