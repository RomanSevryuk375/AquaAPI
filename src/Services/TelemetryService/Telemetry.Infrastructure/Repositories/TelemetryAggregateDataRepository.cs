using Telemetry.Domain.Entities;
using Telemetry.Domain.Interfaces;

namespace Telemetry.Infrastructure.Repositories;

public sealed class TelemetryAggregateDataRepository(SystemDbContext dbContext) 
    : BaseRepository<TelemetryAggregateEntity>(dbContext), ITelemetryAggregateDataRepository
{
}
