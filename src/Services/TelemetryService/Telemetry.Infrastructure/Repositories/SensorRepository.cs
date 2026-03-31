using Telemetry.Domain.Entities;
using Telemetry.Domain.Interfaces;

namespace Telemetry.Infrastructure.Repositories;

public class SensorRepository(SystemDbContext context) 
    : BaseRepository<SensorEntity>(context), ISensorRepository
{
}
