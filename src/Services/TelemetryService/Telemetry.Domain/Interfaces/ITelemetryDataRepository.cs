using Telemetry.Domain.Entities;

namespace Telemetry.Domain.Interfaces;

public interface ITelemetryDataRepository
{
    Task<IReadOnlyList<TelemetryDataEntity>> GetAllAsync(
        BaseSpecification<TelemetryDataEntity>? specification,
        int? skip,
        int? take,
        CancellationToken cancellationToken = default);

    Task<TelemetryDataEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TelemetryDataEntity?> GetByExternalMessageIdAsync(string externalMessageId, CancellationToken cancellationToken = default);
    Task<Guid> AddAsync(TelemetryDataEntity entity, CancellationToken cancellationToken = default);
}
