namespace Core;

public interface IUnitOfWorkFactory : IDisposable
{
    ValueTask<bool> SaveChangesAsync(CancellationToken cT = default);
}