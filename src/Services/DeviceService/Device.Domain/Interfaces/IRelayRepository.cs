using Device.Domain.Entities;

namespace Device.Domain.Interfaces;

public interface IRelayRepository
{
    Task<bool> Exists(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<RelayEntity>> GetAllAsync(
        BaseSpecification<RelayEntity>? specification,
        int? skip,
        int? take,
        CancellationToken cancellationToken);

    Task<RelayEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Guid> AddAsync(RelayEntity entity, CancellationToken cancellationToken);
    Task UpdateAsync(RelayEntity entity, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}