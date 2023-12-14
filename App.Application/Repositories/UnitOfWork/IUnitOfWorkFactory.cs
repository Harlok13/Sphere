namespace App.Application.Repositories.UnitOfWork;

public interface IUnitOfWorkFactory : IDisposable
{
    ValueTask<bool> SaveChangesAsync(CancellationToken cT = default);
}