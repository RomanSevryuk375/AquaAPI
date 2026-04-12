using Control.Domain.Entities;
using Control.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Control.Infrastructure.Repositories;

public class RelayRepository(SystemDbContext dbContext)
    : BaseRepository<RelayEntity>(dbContext), IRelayRepository
{
    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Context.Relays
            .AsNoTracking()
            .AnyAsync(x => x.Id == id, cancellationToken);
    }
}
