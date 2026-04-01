using Telemetry.Application.DTOs;
using Telemetry.Application.Interfaces;
using Telemetry.Domain.Entities;
using Telemetry.Domain.Exceptions;
using Telemetry.Domain.Interfaces;

namespace Telemetry.Application.Services;

public class SensorService(
    IRepository<SensorEntity> repository,
    IUnitOfWork unitOfWork) : ISensorService
{
    public async Task AddSensorAsync(
        SensorRequestDto dto,
        CancellationToken cancellationToken)
    {
        var (sensor, errors) = SensorEntity.Create(
            dto.ControllerId,
            dto.Type,
            dto.Unit,
            0,
            DateTime.UtcNow);

        if (sensor is null)
        {
            throw new DomainValidationException(
                $"Failed to create {nameof(SensorEntity)}: {string.Join(", ", errors)}");
        }

        await repository.AddAsync(sensor, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateSensorAsync(
        Guid id,
        SensorRequestDto dto,
        CancellationToken cancellationToken)
    {
        var existingSensor = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Sensor {id} not found");

        var errors = existingSensor.Update(
            dto.ControllerId,
            dto.Type,
            dto.Unit,
            dto.LastValue,
            dto.UpdatedAt);

        if (errors is not null && errors.Count > 0)
        {
            throw new DomainValidationException(
                $"Update failed: {string.Join(", ", errors)}");
        }

        await repository.UpdateAsync(existingSensor, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteSensorAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var existingSensor = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Sensor {id} not found");

        await repository.DeleteAsync(existingSensor.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
