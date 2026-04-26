using Contracts.Abstractions;
using Device.Domain.Entities;

namespace Device.Domain.Interfaces;

public interface IRelayRepository : IRepository<RelayEntity>
{
    Task<bool> ExistsAsync(
        Guid relayId, 
        CancellationToken cancellationToken);
}