using Contracts.Enums;
using Device.Domain.Entities;
using Device.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Device.Infrastructure.Repositories;

public sealed class RelayCommandsQueueRepository(SystemDbContext dbContext)
    : BaseRepository<RelayCommandsQueueEntity>(dbContext), IRelayCommandsQueueRepository
{
    public async Task<IReadOnlyList<RelayCommandsQueueEntity>> GetPendingByControllerIdAsync(
        Guid controllerId,
        CancellationToken cancellationToken)
    {
        return await Context.RelayCommands
            .AsNoTracking()
            .Where(x =>
                x.ControllerId == controllerId &&
                x.Status == CommandStatusEnum.Pending)
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteCompletedAsync(
        CancellationToken cancellationToken)
    {
        await Context.RelayCommands
            .Where(x => x.Status == CommandStatusEnum.Completed 
                     || x.ExpireAt < DateTime.UtcNow)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
