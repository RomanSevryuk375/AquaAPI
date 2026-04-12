using Control.Domain.Entities;
using Control.Domain.Interfaces;

namespace Control.Infrastructure.Repositories;

public class ScheduleRepository(SystemDbContext dbContext) 
    : BaseRepository<ScheduleEntity>(dbContext), IScheduleRepository
{
}
