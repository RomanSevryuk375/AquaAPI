using Device.Domain.Entities;
using Device.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Device.Infrastructure.Repositories;

public class ControllerRepository(SystemDbContext dbContext) 
    : BaseRepository<ControllerEntity>(dbContext), IControllerRepository
{
    public async Task<ControllerEntity?> GetByMacAddress(string macAddress, CancellationToken cancellationToken)
    {
        return await Context.Controllers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.MacAddress == macAddress, cancellationToken);
    }
}
