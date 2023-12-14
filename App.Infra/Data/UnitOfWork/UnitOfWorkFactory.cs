using App.Application.Repositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace App.Infra.Data.UnitOfWork;

public abstract class UnitOfWorkFactory<TContext> : IUnitOfWorkFactory
    where TContext : DbContext
{
    protected readonly TContext Context;

    protected UnitOfWorkFactory(TContext context)
    {
        Context = context;
    }

    public async ValueTask<bool> SaveChangesAsync(CancellationToken cT)
    {
        return await Context.SaveChangesAsync(cT) > 0;
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}