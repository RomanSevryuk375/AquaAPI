using Device.Domain.Entities;
using Device.Domain.Interfaces;

namespace Device.Infrastructure.Repositories;

public class ControllerRepository(SystemDbContext dbContext) 
    : BaseRepository<ControllerEntity>(dbContext), IControllerRepository
{
}
