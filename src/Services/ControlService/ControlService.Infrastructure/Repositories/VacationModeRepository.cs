using Control.Domain.Entities;
using Control.Domain.Interfaces;

namespace Control.Infrastructure.Repositories;

public class VacationModeRepository(SystemDbContext dbContext)
    : BaseRepository<VacationModeEntity>(dbContext), IVacationModeRepository
{ 

}
