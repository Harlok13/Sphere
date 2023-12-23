using App.Application.Repositories;
using App.Application.Repositories.RoomRepository;
using App.Application.Repositories.UnitOfWork;
using App.Infra.Data.Context;
using App.Infra.Data.UnitOfWork;
using Mediator;
using Microsoft.Extensions.Logging;

namespace App.Infra.Repositories;

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
    // public IRoomRepository RoomRepository => new RoomRepositoryNotifyDecorator(new RoomRepository(Context));
}