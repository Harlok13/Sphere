using System.Linq.Expressions;
using App.Application.Repositories;
using App.Domain.Primitives;
using App.Infra.Data.Context;

namespace App.Infra.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : Entity
{
    protected readonly ApplicationContext Context;

    public GenericRepository(ApplicationContext context)
    {
        Context = context;
    }


    public Task<TEntity> GetByIdAsync(Guid id, CancellationToken cT)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cT)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TEntity>> FindAsync(Expression<Predicate<TEntity>> predicate, CancellationToken cT)
    {
        throw new NotImplementedException();
    }

    public ValueTask<bool> AddAsync(TEntity entity, CancellationToken cT)
    {
        throw new NotImplementedException();
    }

    public ValueTask<bool> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cT)
    {
        throw new NotImplementedException();
    }

    public ValueTask<bool> RemoveAsync(TEntity entity, CancellationToken cT)
    {
        throw new NotImplementedException();
    }

    public ValueTask<bool> RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cT)
    {
        throw new NotImplementedException();
    }
}