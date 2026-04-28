using Contracts.Abstractions;
using Telemetry.Domain.Entities;

namespace Telemetry.Domain.Interfaces;

public interface ITelemetryDataRepository
{
    Task<IReadOnlyList<TelemetryRawEntity>> GetAllAsync(
        BaseSpecification<TelemetryRawEntity>? specification,
        int? skip,
        int? take,
        CancellationToken cancellationToken = default);

    Task<TelemetryRawEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TelemetryRawEntity?> GetByExternalMessageIdAsync(string externalMessageId, CancellationToken cancellationToken = default);
    Task<Guid> AddAsync(TelemetryRawEntity entity, CancellationToken cancellationToken = default);
}
