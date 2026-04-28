using Contracts.Abstractions;
using Telemetry.Domain.Entities;

namespace Telemetry.Domain.Interfaces;

public interface ITelemetryRawDataRepository : IRepository<TelemetryRawEntity>
{
    Task<TelemetryRawEntity?> GetByExternalMessageIdAsync(
        string externalMessageId, 
        CancellationToken cancellationToken);
}
