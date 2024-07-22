using Core;

namespace UserInteraction.Application.Repositories.UnitOfWork;

public interface IUserInteractionUnitOfWork : IUnitOfWorkFactory
{
    IPlayerHistoryRepository PlayerHistoryRepository { get; }
    
    IPlayerInfoRepository PlayerInfoRepository { get; }
}