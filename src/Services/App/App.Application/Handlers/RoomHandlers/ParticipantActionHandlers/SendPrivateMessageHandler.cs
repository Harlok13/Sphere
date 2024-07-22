// using App.SignalR.Commands.RoomCommands.PlayerActionCommands;
// using Mediator;
//
// namespace App.Application.Handlers.RoomHandlers.PlayerActionHandlers;
//
// public class SendPrivateMessageHandler : ICommandHandler<SendPrivateMessageCommand, bool>
// {
//     private readonly ILogger<EndGameHandler> _logger;
//     private readonly IPlayerRepository _playerRepository;
//     private readonly IRoomRepository _roomRepository;
//     private readonly IAppUnitOfWork _unitOfWork;
//     private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
//     private readonly IMediator _mediator;
//
//     public EndGameHandler(
//         ILogger<EndGameHandler> logger,
//         IPlayerRepository playerRepository,
//         IRoomRepository roomRepository,
//         IAppUnitOfWork unitOfWork,
//         IHubContext<GlobalHub, IGlobalHub> hubContext,
//         IMediator mediator)
//     {
//         _logger = logger;
//         _playerRepository = playerRepository;
//         _roomRepository = roomRepository;
//         _unitOfWork = unitOfWork;
//         _hubContext = hubContext;
//         _mediator = mediator;
//     }
//     
//     public async ValueTask<bool> Handle(SendPrivateMessageCommand command, CancellationToken cT)
//     {
//         
//     }
// }