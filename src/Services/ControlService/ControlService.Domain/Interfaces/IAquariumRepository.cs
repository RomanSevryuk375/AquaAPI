using Control.Domain.Entities;

namespace Control.Domain.Interfaces;

public interface IAquariumRepository : IRepository<AquariumEntity>
{
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
    Task<AquariumEntity?> GetByControllerIdAsync(Guid controllerId, CancellationToken cancellationToken);
}