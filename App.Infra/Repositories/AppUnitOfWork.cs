using App.Application.Repositories;
using App.Application.Repositories.UnitOfWork;
using App.Infra.Data.Context;
using App.Infra.Data.UnitOfWork;

namespace App.Infra.Repositories;

public class AppUnitOfWork : UnitOfWorkFactory<ApplicationContext>, IAppUnitOfWork
{
    public AppUnitOfWork(ApplicationContext context) : base(context) { }

    public IPlayerRepository PlayerRepository => new PlayerRepository(Context);
    public IPlayerHistoryRepository PlayerHistoryRepository => new PlayerHistoryRepository(Context);
    public IPlayerStatisticRepository PlayerStatisticRepository => new PlayerStatisticRepository(Context);
    public IRoomRepository RoomRepository => new RoomRepository(Context);
}