using BuildingBlocks.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Infrastructure.Data;

public abstract class BaseRepository<TDbContext, TEntity>(TDbContext dbContext)
    : IRepository<TEntity>
    where TEntity : class, IEntity
    where TDbContext : DbContext
{
    private readonly DbSet<TEntity> _set = dbContext.Set<TEntity>();
    protected TDbContext Context => dbContext;

    public async Task<Guid> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _set.AddAsync(entity, cancellationToken);

        return entity.Id;
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _set.FindAsync([id], cancellationToken: cancellationToken);

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _set.Where(x => x.Id == id).ExecuteDeleteAsync(cancellationToken);
}
