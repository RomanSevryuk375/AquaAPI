using Contracts.Enums;
using Device.Application.DTOs.Sensor;

namespace Device.Application.Interfaces;

public interface ISensorService
{
    Task<IReadOnlyList<SensorResponseDto>> GetAllSensorsAsync(
        SensorFilterDto filter,
        int? skip,
        int? take,
        CancellationToken cancellationToken);

    Task<SensorResponseDto> GetSensorByIdAsync(
        Guid id, 
        CancellationToken cancellationToken);

    Task<Guid> AddSensorAsync(
        SensorRequestDto request, 
        CancellationToken cancellationToken);

    Task UpdateSensorAsync(
        Guid id,
        SensorUpdateRequestDto updateRequestDto,
        CancellationToken cancellationToken);

    Task DeleteSensorAsync(
        Guid id, 
        CancellationToken cancellationToken);

    Task SetSensorStateAsync(
        Guid id,
        SensorStateEnum state,
        CancellationToken cancellationToken);

    Task ProcessTelemetryBatchAsync(
        TelemetryDataRequest request,
        CancellationToken cancellationToken);
}
