using Telemetry.Application.DTOs;

namespace Telemetry.Application.Interfaces;

public interface ISensorService
{
    Task AddSensorAsync(SensorRequestDto dto, CancellationToken cancellationToken);
    Task DeleteSensorAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateSensorAsync(Guid id, SensorRequestDto dto, CancellationToken cancellationToken);
}