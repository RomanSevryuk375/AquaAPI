using Device.Domain.Entities;

namespace Device.Domain.Interfaces;

public interface IControllerRepository
{
    Task<IEnumerable<ControllerEntity>> GetAllAsync(
        BaseSpecification<ControllerEntity>? specification,
        int? skip,
        int? take,
        CancellationToken cancellationToken);

    Task<ControllerEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Guid> AddAsync(ControllerEntity entity, CancellationToken cancellationToken);
    Task UpdateAsync(ControllerEntity entity, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
