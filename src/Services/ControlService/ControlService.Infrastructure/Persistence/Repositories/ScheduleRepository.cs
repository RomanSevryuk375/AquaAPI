using BuildingBlocks.Infrastructure.Data;
using Control.Domain.Entities;
using Control.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Control.Infrastructure.Persistence.Repositories;

public sealed class ScheduleRepository(ControlDbContext dbContext)
    : BaseRepository<ControlDbContext, Schedule>(dbContext), IScheduleRepository
{
    public async Task<IReadOnlyList<Schedule>> GetActiveSchedules(
        CancellationToken cancellationToken = default)
    {
        return await Context.Schedules
            .Where(x => x.IsEnabled)
            .ToListAsync(cancellationToken);
    }
}
