using Contracts.Events.TelemetryEvents;
using Contracts.Exceptions;
using MassTransit;
using Telemetry.Application.DTOs;
using Telemetry.Application.Interfaces;
using Telemetry.Domain.Entities;
using Telemetry.Domain.Interfaces;
using Telemetry.Domain.SpecificationParams;
using Telemetry.Domain.Specifications;

namespace Telemetry.Application.Services;

public class TelemetryDataService(
    ITelemetryDataRepository telemetryRepository,
    ISensorRepository sensorRepository,
    IPublishEndpoint publishEndpoint,
    IUnitOfWork unitOfWork) : ITelemetryDataService
{
    public async Task<IEnumerable<TelemetryDataResponse>> GetAllDataAsync(
        TelemetryDataFilterDto filter,
        int skip,
        int take,
        CancellationToken cancellationToken)
    {
        var specification = new TelemetryFilterSpecification(
            new TelemetryFilterParams
            {
                SensorId = filter.SensorId,
                From = filter.From,
                To = filter.To,
            });

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
        }).ToList();
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
        TelemetryReportedFromHardwareEvent telemetry,
        CancellationToken cancellationToken)
    {
        var existingSensor = await sensorRepository
            .GetByIdAsync(telemetry.SensorId, cancellationToken);

        if (existingSensor is null)
        {
            return;
        }

        var existingTelemetry = await telemetryRepository
            .GetByExternalMessageIdAsync(telemetry.ExternalMessageId, cancellationToken);

        if (existingTelemetry is not null)
        {
            return;
        }

        var (telemetryData, errors) = TelemetryDataEntity.Create(
            telemetry.SensorId,
            telemetry.Value,
            telemetry.ExternalMessageId,
            telemetry.RecordedAt);

        if (errors.Count > 0)
        {
            return;
        }

        existingSensor.UpdateLastValue(telemetry.Value);

        var result = await telemetryRepository.AddAsync(telemetryData!, cancellationToken);

        await sensorRepository.UpdateAsync(existingSensor, cancellationToken); 
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await publishEndpoint.Publish(new TelemetryReceivedEvent
            {
                SensorId = telemetry.SensorId,
                Value = telemetry.Value,
                RecordedAt = telemetry.RecordedAt,
            }, cancellationToken);

        return;
    }
}
