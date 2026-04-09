using Contracts.Enums;
using Contracts.Events.SensorEvents;
using Contracts.Exceptions;
using Device.Application.DTOs.Sensor;
using Device.Application.Interfaces;
using Device.Domain.Entities;
using Device.Domain.Interfaces;
using Device.Domain.SpecificationParams;
using Device.Domain.Specifications;
using MassTransit;

namespace Device.Application.Services;

public class SensorService(
    ISensorRepository sensorRepository,
    IControllerRepository controllerRepository,
    IUnitOfWork unitOfWork,
    IPublishEndpoint publishEndpoint) : ISensorService
{
    public async Task<Guid> AddSensorAsync(
        SensorRequestDto request, 
        CancellationToken cancellationToken)
    {
        var existingController = await controllerRepository
            .GetByIdAsync(request.ControllerId, cancellationToken)
            ?? throw new NotFoundException($"Controller {request.ControllerId} not found");

        var (sensor, errors) = SensorEntity.Create(
            request.ControllerId,
            request.HardwarePin,
            request.Type,
            request.Unit);

        if (sensor is null)
        {
            throw new DomainValidationException(
                $"Failed to create {nameof(SensorEntity)}: {string.Join(", ", errors)}");
        }

        var result = await sensorRepository.AddAsync(sensor, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await publishEndpoint.Publish(new SensorCreatedEvent
        {
            Id = sensor.Id,
            ControllerId = sensor.ControllerId,
            Type = sensor.Type,
            State = sensor.State,
            Unit = sensor.Unit,
            CreatedAt = sensor.CreatedAt
        }, cancellationToken);

        return result;
    }

    public async Task DeleteSensorAsync(
        Guid id, 
        CancellationToken cancellationToken)
    {
        await sensorRepository.DeleteAsync(id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await publishEndpoint.Publish(new SensorDeletedEvent
        {
            Id = id,
        }, cancellationToken);
    }

    public async Task<IReadOnlyList<SensorResponseDto>> GetAllSensorsAsync(
        SensorFilterDto filter,
        int? skip,
        int? take, 
        CancellationToken cancellationToken)
    {
        var specification = new SensorFilterSpecification(
            new SensorFilterParams 
            {
                ControllerId = filter.ControllerId,
                Type = filter.Type,
                State = filter.State,
            });

        var sensors = await sensorRepository.GetAllAsync(
            specification,
            skip,
            take,
            cancellationToken);

        return sensors.Select(entity => new SensorResponseDto
        {
            Id = entity.Id,
            ControllerId = entity.ControllerId,
            HardwarePin = entity.HardwarePin,
            Type = entity.Type,
            State = entity.State,
            Unit = entity.Unit,
            CreatedAt = entity.CreatedAt,
        }).ToList();
    }

    public async Task<SensorResponseDto> GetSensorByIdAsync(
        Guid id, 
        CancellationToken cancellationToken)
    {
        var existingSensor = await sensorRepository
            .GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Sensor {id} not found");

        return new SensorResponseDto
        {
            Id = existingSensor.Id,
            ControllerId = existingSensor.ControllerId,
            HardwarePin = existingSensor.HardwarePin,
            Type = existingSensor.Type,
            State = existingSensor.State,
            Unit = existingSensor.Unit,
            CreatedAt = existingSensor.CreatedAt,
        };
    }

    public async Task SetSensorStateAsync(
        Guid id, 
        SensorStateEnum state, 
        CancellationToken cancellationToken)
    {
        var existingSensor = await sensorRepository
            .GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Sensor {id} not found");

        existingSensor.SetState(state);

        await sensorRepository.UpdateAsync(existingSensor, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await publishEndpoint.Publish(new SensorStateChangedEvent
        {
            Id = existingSensor.Id,
            State = existingSensor.State,
        }, cancellationToken);
    }

    public async Task UpdateSensorAsync(
        Guid id,
        SensorUpdateRequestDto updateRequestDto, 
        CancellationToken cancellationToken)
    {
        var existingSensor = await sensorRepository
            .GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Sensor {id} not found");

        var errors = existingSensor.Update(
            updateRequestDto.HardwarePin,
            updateRequestDto.ControllerId,
            updateRequestDto.Type,
            updateRequestDto.Unit);

        if (errors is not null && errors.Count > 0)
        {
            throw new DomainValidationException(
                $"Update failed: {string.Join(", ", errors)}");
        }

        await sensorRepository.UpdateAsync(existingSensor, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await publishEndpoint.Publish(new SensorUpdatedEvent
        {
            Id = existingSensor.Id,
            ControllerId = existingSensor.ControllerId,
            Type = existingSensor.Type,
            Unit = existingSensor.Unit,
            LastValue = 0.0,
            UpdatedAt = DateTime.UtcNow,
            CreatedAt = existingSensor.CreatedAt
        }, cancellationToken);
    }
}
