using Telemetry.Domain.Entities;
using Telemetry.Domain.Interfaces;

namespace Telemetry.Infrastructure.Repositories;

public class TelemetryDataRepository(SystemDbContext context) 
    : BaseRepository<TelemetryDataEntity>(context), ITelemetryDataRepository
{
}
