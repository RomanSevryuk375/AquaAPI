using Microsoft.EntityFrameworkCore;
using Telemetry.Domain.Entities;
using Telemetry.Domain.Interfaces;

namespace Telemetry.Infrastructure.Repositories;

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
