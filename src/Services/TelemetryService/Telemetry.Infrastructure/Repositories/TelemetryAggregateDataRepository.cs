using Telemetry.Domain.Entities;

namespace Telemetry.Infrastructure.Repositories;

public sealed class TelemetryAggregateDataRepository(SystemDbContext dbContext) 
    : BaseRepository<TelemetryAggregateEntity>(dbContext), ITelemetryAggregateDataRepository
{
}
