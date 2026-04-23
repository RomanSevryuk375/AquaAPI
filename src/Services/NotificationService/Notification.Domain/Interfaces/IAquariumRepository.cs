using Notification.Domain.Entities;

namespace Notification.Domain.Interfaces;

public interface IAquariumRepository : IRepository<AquariumEntity>
{
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
    Task<AquariumEntity?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}
