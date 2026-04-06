using Device.Domain.Entities;

namespace Device.Domain.Interfaces;

public interface ISensorRepository
{
    Task<bool> Exists(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<SensorEntity>> GetAllAsync(
        BaseSpecification<SensorEntity>? specification,
        int? skip,
        int? take,
        CancellationToken cancellationToken);

    Task<SensorEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Guid> AddAsync(SensorEntity entity, CancellationToken cancellationToken);
    Task UpdateAsync(SensorEntity entity, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
