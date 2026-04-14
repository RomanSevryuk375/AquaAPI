using Microsoft.EntityFrameworkCore;
using Telemetry.Domain.Entities;
using Telemetry.Domain.Interfaces;

namespace Telemetry.Infrastructure.Repositories;

public class TelemetryDataRepository(SystemDbContext dbContext)
    : BaseRepository<TelemetryDataEntity>(dbContext), ITelemetryDataRepository
{
    public async Task<TelemetryDataEntity?> GetByExternalMessageIdAsync(
        string externalMessageId, 
        CancellationToken cancellationToken = default)
    {
        return await Context.TelemetryDatas
            .AsNoTracking()
            .FirstOrDefaultAsync(x => 
                x.ExternalMessageId == externalMessageId, cancellationToken);
    }
}
