using Microsoft.EntityFrameworkCore;
using Telemetry.Domain.Entities;
using Telemetry.Domain.Interfaces;

namespace Telemetry.Infrastructure.Repositories;

public sealed class TelemetryRawDataRepository(SystemDbContext dbContext)
    : BaseRepository<TelemetryRawEntity>(dbContext), ITelemetryDataRepository
{
    public async Task<TelemetryRawEntity?> GetByExternalMessageIdAsync(
        string externalMessageId, 
        CancellationToken cancellationToken = default)
    {
        return await Context.TelemetryRawData
            .AsNoTracking()
            .FirstOrDefaultAsync(x => 
                x.ExternalMessageId == externalMessageId, cancellationToken);
    }
}
