using Control.Domain.Entities;
using Control.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Control.Infrastructure.Repositories;

public class SensorRepository(SystemDbContext dbContext)
    : BaseRepository<SensorEntity>(dbContext), ISensorRepository
{
    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Context.Sensors
            .AsNoTracking()
            .AnyAsync(x => x.Id == id, cancellationToken);
    }
}
