using Device.Domain.Entities;
using Device.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Device.Infrastructure.Repositories;

public class SensorRepository(SystemDbContext context)
    : BaseRepository<SensorEntity>(context), ISensorRepository
{
    public async Task<bool> Exists(Guid id, CancellationToken cancellationToken)
    {
        return await context.Sensors
            .AsNoTracking()
            .AnyAsync(x => x.Id == id, cancellationToken);
    }
}
