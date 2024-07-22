using Core.UnitOfWork;
using Mediator;
using Microsoft.Extensions.Logging;
using UserInteraction.Application.Repositories;
using UserInteraction.Application.Repositories.UnitOfWork;
using UserInteraction.Infrastructure.Context;

namespace UserInteraction.Infrastructure.Repositories;

public class UserInteractionUnitOfWork : UnitOfWorkFactory<UserInteractionContext>, IUserInteractionUnitOfWork
{
    public UserInteractionUnitOfWork(
        UserInteractionContext context,
        ILogger<UnitOfWorkFactory<UserInteractionContext>> logger,
        IPublisher publisher) : 
        base(context, logger, publisher) { }

    public IPlayerHistoryRepository PlayerHistoryRepository => new PlayerHistoryRepository(Context);
    public IPlayerInfoRepository PlayerInfoRepository => new PlayerInfoRepository(Context);
}