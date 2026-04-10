using Telemetry.Domain.Entities;
using Telemetry.Domain.Interfaces;

namespace Telemetry.Infrastructure.Repositories;

public class TelemetryDataRepository(SystemDbContext dbContext) 
    : BaseRepository<TelemetryDataEntity>(dbContext), ITelemetryDataRepository
{

}
