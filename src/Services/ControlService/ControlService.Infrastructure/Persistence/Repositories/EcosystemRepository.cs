using BuildingBlocks.Infrastructure.Data;
using Control.Domain.Entities;
using Control.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Control.Infrastructure.Persistence.Repositories;

public sealed class EcosystemRepository(ControlDbContext dbContext)
    : BaseRepository<ControlDbContext, Ecosystem>(dbContext), IEcosystemRepository
{
    public async Task<bool> ExistsAsync(
        Guid ecosystemId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Ecosystems
            .AsNoTracking()
            .AnyAsync(x => x.Id == ecosystemId, cancellationToken);
    }

    public async Task<Ecosystem?> GetByControllerIdAsync(
        Guid controllerId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Ecosystems
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ControllerId == controllerId, cancellationToken);
    }
}
