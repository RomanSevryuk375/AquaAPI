using BuildingBlocks.Domain.Abstractions;
using Control.Domain.Entities;

namespace Control.Domain.Interfaces;

public interface IEcosystemRepository : IRepository<Ecosystem>
{
    public Task<bool> ExistsAsync(
        Guid ecosystemId,
        CancellationToken cancellationToken = default);

    public Task<Ecosystem?> GetByControllerIdAsync(
        Guid controllerId,
        CancellationToken cancellationToken = default);
}
