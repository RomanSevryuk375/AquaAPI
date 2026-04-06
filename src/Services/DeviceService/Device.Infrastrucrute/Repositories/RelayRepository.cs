using Device.Domain.Entities;
using Device.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Device.Infrastructure.Repositories;

public class RelayRepository(SystemDbContext context)
    : BaseRepository<RelayEntity>(context), IRelayRepository
{
    public async Task<bool> Exists(Guid id, CancellationToken cancellationToken)
    {
        return await context.Relays
            .AsNoTracking()
            .AnyAsync(x => x.Id == id, cancellationToken);
    }
}
