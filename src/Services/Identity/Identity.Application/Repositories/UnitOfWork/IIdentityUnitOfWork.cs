using Core;

namespace Identity.Application.Repositories.UnitOfWork;

public interface IIdentityUnitOfWork : IUnitOfWorkFactory
{
    IApplicationUserRepository ApplicationUserRepository { get; }
}