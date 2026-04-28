using Telemetry.Domain.Entities;

namespace Telemetry.Infrastructure.Repositories;

public sealed class EcosystemRepository(SystemDbContext dbContext) 
    : BaseRepository<EcosystemEntity>(dbContext), IEcosystemRepository
{
}
