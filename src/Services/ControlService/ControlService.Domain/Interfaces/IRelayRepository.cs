using Control.Domain.Entities;

namespace Control.Domain.Interfaces;

public interface IRelayRepository : IRepository<RelayEntity>
{
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
}