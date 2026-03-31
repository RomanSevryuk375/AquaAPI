using System.Linq.Expressions;

namespace Telemetry.Domain.Interfaces;

public interface IRepository<T> where T : class, IEntity
{
    Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter,
        int? skip,
        int? take,
        CancellationToken cancellationToken = default);

    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    void Update(T entity);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
