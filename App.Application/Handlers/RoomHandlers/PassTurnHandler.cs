using App.Application.Extensions;
using App.Application.Repositories.UnitOfWork;
using App.Domain.Entities.RoomEntity;
using App.SignalR.Commands.RoomCommands;
using Mediator;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.RoomHandlers;

public class PassTurnHandler : ICommandHandler<PassTurnCommand, bool>
{
    private readonly ILogger<PassTurnHandler> _logger;
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public PassTurnHandler(
        ILogger<PassTurnHandler> logger,
        IAppUnitOfWork unitOfWork,
        IMediator mediator)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }
    
    public async ValueTask<bool> Handle(PassTurnCommand command, CancellationToken cT)
    {
        command.Deconstruct(out Guid roomId);

        var roomResult = await _unitOfWork.RoomRepository.GetByIdAsync(roomId, cT);
        if (!roomResult.TryFromResult(out Room? room, out var roomErrors))
        {
            foreach(var error in roomErrors) _logger.LogError(error.Message);

            return false;
        }

        var passTurnResult = room!.PassTurn();
        if (passTurnResult)
        {
            return await _unitOfWork.SaveChangesAsync(cT);
        }
        // var players = room.Players;

        // var playersInGame = players.Where(p => p.InGame).ToImmutableList().Count;
        // if (playersInGame < 2)
        // {
        //     // await _mediator.Send(new EndGameCommand(), cT);  TODO: finish
        // }


        // if (players.Any(p => p.MoveStatus == MoveStatus.None))
        // {
        //     players
        //         .FirstOrDefault(p => p.MoveStatus == MoveStatus.None)!
        //         .SetMove(true);
        //
        //     // await _mediator.Send(new StartTimerCommand(new StartTimerRequest(room.Id, playerId), connectionId), cT);
        //     
        //     return await _unitOfWork.SaveChangesAsync(cT);
        // }

        _logger.LogInformation("Invoking command {CommandName} with argument {Argument}.",
            nameof(EndCycleCommand),
            new {RoomId = roomId});
        await _mediator.Send(new EndCycleCommand(RoomId: roomId), cT);  // TODO: finish
        return false;
    }
}

// EndCycleCommand
// StartCycleCommand
// EndGameCommand