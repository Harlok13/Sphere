using App.Application.Repositories;
using App.Application.Repositories.RoomRepository;
using App.Application.Repositories.UnitOfWork;
using Infrastructure.Data.Context;
using Infrastructure.Data.UnitOfWork;
using Mediator;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class AppUnitOfWork : UnitOfWorkFactory<ApplicationContext>, IAppUnitOfWork
{
    public AppUnitOfWork(
        ApplicationContext context,
        ILogger<UnitOfWorkFactory<ApplicationContext>> logger,
        IPublisher publisher
    ) : base(context, logger, publisher) { }

    public IPlayerRepository PlayerRepository => new PlayerRepository(Context);
    public IPlayerHistoryRepository PlayerHistoryRepository => new PlayerHistoryRepository(Context);
    public IPlayerInfoRepository PlayerInfoRepository => new PlayerInfoRepository(Context);
    public IRoomRepository RoomRepository => new RoomRepository(Context);
    public IFriendsRepository FriendsRepository => new FriendsRepository(Context);
}