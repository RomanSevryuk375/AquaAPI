using Contracts.Abstractions;
using Telemetry.Domain.Entities;

namespace Telemetry.Domain.Interfaces;

public interface ITelemetryDataRepository : IRepository<TelemetryRawEntity>
{
    Task<TelemetryRawEntity?> GetByExternalMessageIdAsync(
        string externalMessageId, 
        CancellationToken cancellationToken);
}
