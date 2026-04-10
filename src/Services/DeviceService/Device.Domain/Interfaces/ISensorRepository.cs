using Device.Domain.Entities;

namespace Device.Domain.Interfaces;

public interface ISensorRepository : IRepository<SensorEntity>
{
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
}
