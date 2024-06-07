using Core;

namespace GameInteraction.Application.Repositories.UnitOfWork;

public interface IGameInteractionUnitOfWork : IUnitOfWorkFactory
{
    IPlayerRepository PlayerRepository { get; }

    IRoomRepository RoomRepository { get; }
}