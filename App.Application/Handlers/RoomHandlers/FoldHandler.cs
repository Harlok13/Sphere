// using App.Application.Repositories;
// using App.Application.Repositories.RoomRepository;
// using App.Application.Repositories.UnitOfWork;
// using App.Contracts.Mapper;
// using App.Domain.Enums;
// using App.SignalR.Commands;
// using App.SignalR.Commands.RoomCommands;
// using App.SignalR.Hubs;
// using Mediator;
// using Microsoft.AspNetCore.SignalR;
// using Microsoft.Extensions.Logging;
//
// namespace App.Application.Handlers.RoomHandlers;
//
// public class FoldHandler : ICommandHandler<FoldCommand, bool>
// {
//     private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
//     private readonly IPublisher _publisher;
//     private readonly ILogger<FoldHandler> _logger;
//     private readonly IRoomRepository _roomRepository;
//     private readonly IPlayerInfoRepository _playerInfoRepository;
//     private readonly IPlayerRepository _playerRepository;
//     private readonly IAppUnitOfWork _unitOfWork;
//     private readonly IMediator _mediator;
//
//     public FoldHandler(
//         ILogger<FoldHandler> logger,
//         IRoomRepository roomRepository,
//         IPlayerInfoRepository playerInfoRepository,
//         IAppUnitOfWork unitOfWork,
//         IMediator mediator, 
//         IPlayerRepository playerRepository,
//         IHubContext<GlobalHub, IGlobalHub> hubContext,
//         IPublisher publisher
//     )
//     {
//         _logger = logger;
//         _roomRepository = roomRepository;
//         _playerInfoRepository = playerInfoRepository;
//         _unitOfWork = unitOfWork;
//         _mediator = mediator;
//         _playerRepository = playerRepository;
//         _hubContext = hubContext;
//         _publisher = publisher;
//     }
//     
//     public async ValueTask<bool> Handle(FoldCommand command, CancellationToken cT)
//     {
//         command.Deconstruct(out Guid roomId, out Guid playerId);
//
//         var player = await _playerRepository.GetPlayerByIdAsync(playerId, cT);
//         if (!player.Move)
//         {
//             // TODO: notify 
//         }
//
//         // TODO: player.Fold();
//         player.ResetCards();  // TODO: mb not? it need to the statistic or history
//         player.SetMoveStatus(MoveStatus.Fold);
//         player.SetMove(false);
//         player.SetInGame(false);  // TODO: event changed cards (reset)
//
//         // await _hubContext.Clients.Client(player.ConnectionId).ReceiveOwn_ChangedPlayerInGame(false, cT);
//         // await _hubContext.Clients.Group(roomId.ToString()).ReceiveGroup_PlayerInGame(playerId, false, cT);
//         // var playerResponse = PlayerMapper.MapPlayerToPlayerResponse(player);
//         // await _hubContext.Clients.Group(roomId.ToString()).ReceiveGroup_ChangedPlayerReadiness(playerResponse, cT);
//         await _publisher.Publish(new NotifyFoldEvent(), cT);  // TODO: send message to the game history
//         await _hubContext.Clients.Client(player.ConnectionId).ReceiveOwn_Fold(cT);
//         await _hubContext.Clients.Group(roomId.ToString()).ReceiveGroup_Fold(playerId, cT);
//
//         await _mediator.Send(new PassTurnCommand(roomId), cT);
//
//         await _unitOfWork.SaveChangesAsync(cT);
//         return true;
//     }
// }