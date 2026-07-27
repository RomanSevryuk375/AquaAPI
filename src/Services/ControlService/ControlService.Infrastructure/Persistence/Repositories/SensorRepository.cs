using BuildingBlocks.Infrastructure.Data;
using Control.Domain.Entities;
using Control.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Control.Infrastructure.Persistence.Repositories;

public sealed class SensorRepository(ControlDbContext dbContext)
    : BaseRepository<ControlDbContext, Sensor>(dbContext), ISensorRepository
{
    public async Task<bool> ExistsAsync(
        Guid sensorId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Sensors
            .AsNoTracking()
            .AnyAsync(x => x.Id == sensorId, cancellationToken);
    }

    public async Task<IReadOnlyList<Sensor>> GetManyByIdsAsync(
        IEnumerable<Guid> sensorIds,
        CancellationToken cancellationToken = default)
    {
        return await Context.Sensors
            .AsNoTracking()
            .Where(x => sensorIds.Contains(x.Id))
            .ToListAsync(cancellationToken);
    }
}
