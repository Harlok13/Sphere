using Core.UnitOfWork;
using Identity.Application.Repositories;
using Identity.Application.Repositories.UnitOfWork;
using Identity.Domain.Entities;
using Identity.Infrastructure.Context;
using Mediator;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Identity.Infrastructure.Repositories;

public class IdentityUnitOfWork : UnitOfWorkFactory<IdentityContext>, IIdentityUnitOfWork
{
    private readonly UserManager<User> _userManager;

    public IdentityUnitOfWork(
        UserManager<User> userManager,
        IdentityContext context,
        ILogger<UnitOfWorkFactory<IdentityContext>> logger,
        IPublisher publisher) : base(context, logger, publisher)
    {
        _userManager = userManager;
    }

    public IIdentityUserRepository IdentityUserRepository => new IdentityUserRepository(_userManager, Context);
}