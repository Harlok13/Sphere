using App.Application.Repositories.RoomRepository;

namespace App.Application.Repositories.UnitOfWork;

public interface IAppUnitOfWork : IUnitOfWorkFactory
{
    IPlayerRepository PlayerRepository { get; }
    
    IPlayerHistoryRepository PlayerHistoryRepository { get; }
    
    IPlayerInfoRepository PlayerInfoRepository { get; }
    
    IRoomRepository RoomRepository { get; }
}