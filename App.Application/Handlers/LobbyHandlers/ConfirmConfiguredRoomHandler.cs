using App.Application.Repositories.RoomRepository;
using App.Application.Repositories.UnitOfWork;
using App.SignalR.Commands.LobbyCommands;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.LobbyHandlers;

public class ConfirmConfiguredRoomHandler : ICommandHandler<ConfirmConfiguredRoomCommand, bool>
{
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly ILogger<ConfirmConfiguredRoomHandler> _logger;
    private readonly IRoomRepository _roomRepository;
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public ConfirmConfiguredRoomHandler(
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        ILogger<ConfirmConfiguredRoomHandler> logger,
        IRoomRepository roomRepository,
        IAppUnitOfWork unitOfWork,
        IMediator mediator
    )
    {
        _hubContext = hubContext;
        _logger = logger;
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }
    
    public async ValueTask<bool> Handle(ConfirmConfiguredRoomCommand command, CancellationToken cT)
    {
        var (requestRoom, userId, userName, connectionId, isLeader) = command;

        // await Task.Delay(1, cT);
        return true;
    }
}