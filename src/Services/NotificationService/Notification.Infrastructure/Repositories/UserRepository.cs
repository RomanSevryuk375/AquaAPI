using Microsoft.EntityFrameworkCore;
using Notification.Domain.Entities;
using Notification.Domain.Interfaces;

namespace Notification.Infrastructure.Repositories;

public class UserRepository(SystemDbContext dbContext)
    : BaseRepository<UserEntity>(dbContext), IUserRepository
{
    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Context.Users
            .AsNoTracking()
            .AnyAsync(x => x.Id == id, cancellationToken);
    }
}
