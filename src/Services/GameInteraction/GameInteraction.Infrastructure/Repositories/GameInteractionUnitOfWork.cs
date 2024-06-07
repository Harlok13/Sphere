using Core.UnitOfWork;
using GameInteraction.Application.Repositories;
using GameInteraction.Application.Repositories.UnitOfWork;
using GameInteraction.Infrastructure.Context;
using Mediator;
using Microsoft.Extensions.Logging;

namespace GameInteraction.Infrastructure.Repositories;

public class GameInteractionUnitOfWork : UnitOfWorkFactory<GameInteractionContext>, IGameInteractionUnitOfWork
{
    public GameInteractionUnitOfWork(
        GameInteractionContext context,
        ILogger<UnitOfWorkFactory<GameInteractionContext>> logger,
        IPublisher publisher) : 
        base(context, logger, publisher) { }

    public IPlayerRepository PlayerRepository => new PlayerRepository(Context);
    public IRoomRepository RoomRepository => new RoomRepository(Context);
}