using Telemetry.Domain.Entities;

namespace Telemetry.Domain.Interfaces;

public interface ISensorRepository
{
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
    Task<Guid> AddAsync(SensorEntity entity, CancellationToken cancellationToken);
    Task UpdateAsync(SensorEntity entity, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<SensorEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
