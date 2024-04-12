// using App.Application.Extensions;
// using App.Application.Messages;
// using App.Application.Repositories.UnitOfWork;
// using App.Contracts.Data;
// using App.Contracts.Mapper;
// using App.Domain.Entities;
// using App.Domain.Entities.PlayerInfoEntity;
// using App.SignalR.Commands.RoomCommands.PlayerActionCommands;
// using App.SignalR.Events;
// using App.SignalR.Hubs;
// using Mediator;
// using Microsoft.AspNetCore.SignalR;
// using Microsoft.Extensions.Logging;
//
// namespace App.Application.Handlers.RoomHandlers.ParticipantActionHandlers;
//
// public class AddToFriendsHandler : ICommandHandler<AddToFriendsCommand, bool>
// {
//     private readonly ILogger<AddToFriendsHandler> _logger;
//     private readonly IAppUnitOfWork _unitOfWork;
//     private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
//     private readonly IMediator _mediator;
//     private readonly IPublisher _publisher;
//
//     public AddToFriendsHandler(
//         ILogger<AddToFriendsHandler> logger,
//         IAppUnitOfWork unitOfWork,
//         IHubContext<GlobalHub, IGlobalHub> hubContext,
//         IMediator mediator,
//         IPublisher publisher)
//     {
//         _logger = logger;
//         _unitOfWork = unitOfWork;
//         _hubContext = hubContext;
//         _mediator = mediator;
//         _publisher = publisher;
//     }
//
//     public async ValueTask<bool> Handle(AddToFriendsCommand command, CancellationToken cT)
//     {
//         command.Request.Deconstruct(out Guid playerId, out Guid friendId);
//
//         var playerInfoResult = await _unitOfWork.PlayerInfoRepository.GetPlayerInfoWithFriendsAsync(playerId, cT);
//         if (!playerInfoResult.TryFromResult(out PlayerInfo? playerInfo, out var playerInfoErrors))
//         {
//             foreach (var error in playerInfoErrors) _logger.LogError(error.Message);
//         
//             await _publisher.Publish(new UserNotificationEvent(
//                     NotificationText: NotificationMessages.SomethingWentWrong(),
//                     TargetId: playerId),
//                 cT);
//         
//             return false;
//         }
//
//         var friendResult = await _unitOfWork.PlayerInfoRepository.GetPlayerInfoByIdAsNoTrackingAsync(friendId, cT);
//         if (!friendResult.TryFromResult(out PlayerInfoDto? friendDto, out var friendErrors))
//         {
//             foreach (var error in friendErrors) _logger.LogError(error.Message);
//         
//             await _publisher.Publish(new UserNotificationEvent(
//                     NotificationText: NotificationMessages.SomethingWentWrong(),
//                     TargetId: playerId),
//                 cT);
//         
//             return false;
//         }
//         
//         var friend = PlayerMapper.
//         playerInfo.AddToFriends(friendDto)
//
//         // var friend = PlayerMapper.MapPlayerInfoDtoToPlayerInfo(friendDto);
//         // playerInfo!.AddToFriends(friendId);
//         // var friends = Friends.Create(playerId: playerId, friendId: friendId);
//         // await _unitOfWork.FriendsRepository.AddToFriendsAsync(friends, cT);
//
//         return await _unitOfWork.SaveChangesAsync(cT);
//     }
// }