using System.Linq.Expressions;
using App.Domain.Primitives;

namespace App.Application.Repositories;

public interface IGenericRepository<TEntity> where TEntity : Entity
{
    Task<TEntity> GetByIdAsync(Guid id, CancellationToken cT);

    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cT);

    Task<IEnumerable<TEntity>> FindAsync(Expression<Predicate<TEntity>> predicate, CancellationToken cT);

    ValueTask<bool> AddAsync(TEntity entity, CancellationToken cT);

    ValueTask<bool> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cT);

    ValueTask<bool> RemoveAsync(TEntity entity, CancellationToken cT);

    ValueTask<bool> RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cT);
}