using Device.Domain.Entities;

namespace Device.Domain.Interfaces;

public interface IRelayRepository : IRepository<RelayEntity>
{
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
}