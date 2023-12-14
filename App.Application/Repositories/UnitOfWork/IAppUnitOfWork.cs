namespace App.Application.Repositories.UnitOfWork;

public interface IAppUnitOfWork : IUnitOfWorkFactory
{
    IPlayerRepository PlayerRepository { get; }
    
    IPlayerHistoryRepository PlayerHistoryRepository { get; }
    
    IPlayerStatisticRepository PlayerStatisticRepository { get; }
    
    IRoomRepository RoomRepository { get; }
}