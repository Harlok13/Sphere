using App.Contracts.Data;
using App.Contracts.Responses;
using App.Contracts.Responses.PlayerResponses;
using App.Contracts.Responses.RoomResponses;

namespace App.SignalR.Hubs;

public interface IGlobalHub
{
    Task ReceiveAll_CreatedRoom(CreatedRoomResponse response, CancellationToken cT);
    Task ReceiveGroup_AddedPlayer(AddedPlayerResponse response, CancellationToken cT);
    Task ReceiveOwn_CreatedPlayer(CreatedPlayerResponse response, CancellationToken cT);
    Task ReceiveOwn_RemoveFromRoom(CancellationToken cT);
    Task ReceiveGroup_RemovedPlayer(RemovedPlayerResponse response, CancellationToken cT);
    Task ReceiveAll_RemovedRoom(Guid roomId, CancellationToken cT);
    Task ReceiveGroup_ChangedPlayerReadiness(ChangedPlayerReadinessResponse response, CancellationToken cT);
    Task ReceiveOwn_ChangedPlayerReadiness(ChangedPlayerReadinessResponse response, CancellationToken cT);
    Task ReceiveOwn_SelectStartGameMoney(SelectStartGameMoneyResponse response, CancellationToken cT);
    Task ReceiveOwn_NotEnoughMoneyNotification(NotEnoughMoneyNotificationResponse response, CancellationToken cT);
    Task ReceiveOwn_ChangedPlayerInfoMoney(ChangedPlayerInfoMoneyResponse response, CancellationToken cT);
    Task ReceiveOwn_AddedCard(AddedCardResponse response, CancellationToken cT);
    Task ReceiveGroup_AddedCard(AddedCardResponse response, CancellationToken cT);
    Task ReceiveGroup_ChangedRoomBank(ChangedRoomBankResponse response, CancellationToken cT);
    Task ReceiveOwn_ChangedPlayerMove(ChangedPlayerMoveResponse response, CancellationToken cT);
    Task ReceiveGroup_StartGameErrorNotification(string? errorMsg, CancellationToken cT);
    Task ReceiveOwn_ChangedPlayerMoney(ChangedPlayerMoneyResponse response, CancellationToken cT);
    Task ReceiveGroup_ChangedPlayerMoney(ChangedPlayerMoneyResponse response, CancellationToken cT);
    Task ReceiveGroup_StartGame(CancellationToken cT);
    Task ReceiveOwn_UpdatedTimer(int seconds, CancellationToken cT);
    Task ReceiveOwn_ChangedPlayerInGame(ChangedPlayerInGameResponse response, CancellationToken cT);
    Task ReceiveGroup_ChangedPlayerInGame(ChangedPlayerInGameResponse response, CancellationToken cT);
    Task ReceiveAll_ChangedRoomStatus(ChangedRoomStatusResponse response, CancellationToken cT);
    Task ReceiveAll_ChangedRoomPlayersInRoom(ChangedRoomPlayersInRoomResponse response, CancellationToken cT);
    Task ReceiveOwn_ChangedPlayerIsLeader(ChangedPlayerIsLeaderResponse response, CancellationToken cT);
    Task ReceiveGroup_ChangedPlayerIsLeader(ChangedPlayerIsLeaderResponse response, CancellationToken cT);
    Task ReceiveAll_ChangedRoomRoomName(ChangedRoomRoomNameResponse response, CancellationToken cT);
    Task ReceiveGroup_ChangedRoomRoomName(ChangedRoomRoomNameResponse response, CancellationToken cT);
    Task ReceiveAll_ChangedRoomAvatarUrl(ChangedRoomAvatarUrlResponse response, CancellationToken cT);
}