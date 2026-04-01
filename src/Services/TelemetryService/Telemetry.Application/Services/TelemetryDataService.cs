using Telemetry.Application.DTOs;
using Telemetry.Application.Interfaces;
using Telemetry.Domain.Entities;
using Telemetry.Domain.Exceptions;
using Telemetry.Domain.Interfaces;
using Telemetry.Domain.Specifications;

namespace Telemetry.Application.Services;

public class TelemetryDataService(
    IRepository<TelemetryDataEntity> telemetryRepository,
    ISensorRepository sensorRepository,
    IUnitOfWork unitOfWork) : ITelemetryDataService
{
    public async Task<IEnumerable<TelemetryDataResponse>> GetAllDataAsync(
        TelemetryDataFilterDto filter,
        int skip,
        int take,
        CancellationToken cancellationToken)
    {
        var specification = new TelemetryFilterSpecification(
            filter.SensorId, filter.From, filter.To);

        var entities = await telemetryRepository.GetAllAsync(
            specification,
            skip,
            take,
            cancellationToken);

        return entities.Select(entity => new TelemetryDataResponse
        {
            Id = entity.Id,
            SensorId = entity.SensorId,
            Value = entity.Value,
            ExternalMessageId = entity.ExternalMessageId,
            RecordedAt = entity.RecordedAt,
            CreatedAt = entity.CreatedAt
        }).ToList(); ;
    }

    public async Task<TelemetryDataResponse> GetDataByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var entity = await telemetryRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"{nameof(TelemetryDataEntity)} not found");

        return new TelemetryDataResponse()
        {
            Id = entity.Id,
            SensorId = entity.SensorId,
            Value = entity.Value,
            ExternalMessageId = entity.ExternalMessageId,
            RecordedAt = entity.RecordedAt,
            CreatedAt = entity.CreatedAt
        };
    }

    public async Task AddDataAsync(
        TelemetryDataRequest dto,
        CancellationToken cancellationToken)
    {
        var sensorExists = await sensorRepository.ExistsAsync(dto.SensorId, cancellationToken);
        if (!sensorExists) throw new NotFoundException($"Sensor {dto.SensorId} not found");

        var (telemetryData, errors) = TelemetryDataEntity.Create(
            dto.SensorId,
            dto.Value,
            dto.ExternalMessageId,
            dto.RecordedAt);

        if (telemetryData is null)
        {
            throw new DomainValidationException(
                $"Failed to create {nameof(TelemetryDataEntity)}: {string.Join(", ", errors)}");
        }

        await telemetryRepository.AddAsync(telemetryData, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
