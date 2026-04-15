using Notification.Domain.Entities;

namespace Notification.Domain.Interfaces;

public interface IUserRepository : IRepository<UserEntity>
{
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
}
