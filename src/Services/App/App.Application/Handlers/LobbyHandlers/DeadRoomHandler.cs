using App.SignalR.Commands.LobbyCommands;
using Mediator;

namespace App.Application.Handlers.LobbyHandlers;

public class DeadRoomHandler : ICommandHandler<DeadRoomCommand, bool>
{
    public async ValueTask<bool> Handle(DeadRoomCommand command, CancellationToken cT)
    {
        command.Deconstruct(out CancellationTokenSource cts);

        var token = cts.Token;
        
        // _ = Task.Run(async () =>
        // {
        //     await Task.Delay()
        // })
        _ = Task.Run(async () =>
        {
            // await Task.Delay(300000, token);
            Thread.Sleep(3000);
            
            // await _roomRepository.RemoveAsync(room.Id, token);
            // _logger.LogInformation("");
        }, token);
        
        await Task.Delay(10, cT);
        return false;
    }
}