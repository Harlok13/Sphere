using App.Application.Repositories.UnitOfWork;
using App.SignalR.Commands.RoomCommands;
using App.SignalR.Commands.RoomCommands.GameActionCommands;
using Mediator;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.RoomHandlers.GameActionHandlers;

public class StayHandler : ICommandHandler<StayCommand, bool>
{
    private readonly ILogger<StayHandler> _logger;
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public StayHandler(
        ILogger<StayHandler> logger,
        IAppUnitOfWork unitOfWork,
        IMediator mediator
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }
    
    public async ValueTask<bool> Handle(StayCommand command, CancellationToken cT)
    {
        command.Request.Deconstruct(out Guid roomId, out Guid playerId);
        _logger.LogInformation($"Invoked method \"Stay\".");

        var player = await _unitOfWork.PlayerRepository.GetPlayerByIdAsync(playerId, cT);
        player.Stay();

        await _unitOfWork.SaveChangesAsync(cT);

        return await _mediator.Send(new PassTurnCommand(roomId), cT);
    }
}