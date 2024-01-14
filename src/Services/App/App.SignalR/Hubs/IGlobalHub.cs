using App.Contracts.Responses;
using App.Contracts.Responses.PlayerInfoResponses;
using App.Contracts.Responses.PlayerResponses;
using App.Contracts.Responses.RoomResponses;

namespace App.SignalR.Hubs;

public interface IGlobalHub
{
    Task ReceiveAll_CreatedRoom(CreatedRoomResponse response, CancellationToken cT);
    Task ReceiveGroup_AddedPlayer(AddedPlayerResponse response, CancellationToken cT);
    Task ReceiveUser_CreatedPlayer(CreatedPlayerResponse response, CancellationToken cT);
    Task ReceiveUser_RemoveFromRoom(CancellationToken cT);
    Task ReceiveGroup_RemovedPlayer(RemovedPlayerResponse response, CancellationToken cT);
    Task ReceiveAll_RemovedRoom(RemovedRoomResponse response, CancellationToken cT);
    Task ReceiveGroup_ChangedPlayerReadiness(ChangedPlayerReadinessResponse response, CancellationToken cT);
    Task ReceiveUser_ChangedPlayerReadiness(ChangedPlayerReadinessResponse response, CancellationToken cT);
    Task ReceiveUser_SelectStartGameMoney(SelectStartGameMoneyResponse response, CancellationToken cT);
    Task ReceiveClient_Notification(NotificationResponse response, CancellationToken cT);
    Task ReceiveUser_Notification(NotificationResponse response, CancellationToken cT);
    Task ReceiveUser_ChangedPlayerInfoMoney(ChangedPlayerInfoMoneyResponse response, CancellationToken cT);
    Task ReceiveUser_AddedCard(AddedCardResponse response, CancellationToken cT);
    Task ReceiveGroup_AddedCard(AddedCardResponse response, CancellationToken cT);
    Task ReceiveGroup_ChangedRoomBank(ChangedRoomBankResponse response, CancellationToken cT);
    Task ReceiveUser_ChangedPlayerMove(ChangedPlayerMoveResponse response, CancellationToken cT);
    Task ReceiveUser_ChangedPlayerMoney(ChangedPlayerMoneyResponse response, CancellationToken cT);
    Task ReceiveGroup_ChangedPlayerMoney(ChangedPlayerMoneyResponse response, CancellationToken cT);
    Task ReceiveUser_ChangedTimer(int seconds, CancellationToken cT);
    Task ReceiveUser_ChangedPlayerInGame(ChangedPlayerInGameResponse response, CancellationToken cT);
    Task ReceiveGroup_ChangedPlayerInGame(ChangedPlayerInGameResponse response, CancellationToken cT);
    Task ReceiveAll_ChangedRoomStatus(ChangedRoomStatusResponse response, CancellationToken cT);
    Task ReceiveAll_ChangedRoomPlayersInRoom(ChangedRoomPlayersInRoomResponse response, CancellationToken cT);
    Task ReceiveUser_ChangedPlayerIsLeader(ChangedPlayerIsLeaderResponse response, CancellationToken cT);
    Task ReceiveGroup_ChangedPlayerIsLeader(ChangedPlayerIsLeaderResponse response, CancellationToken cT);
    Task ReceiveAll_ChangedRoomRoomName(ChangedRoomRoomNameResponse response, CancellationToken cT);
    Task ReceiveGroup_ChangedRoomRoomName(ChangedRoomRoomNameResponse response, CancellationToken cT);
    Task ReceiveAll_ChangedRoomAvatarUrl(ChangedRoomAvatarUrlResponse response, CancellationToken cT);
    Task ReceiveUser_TimeOut(CancellationToken cT);
    Task ReceiveUser_ReconnectToRoom(ReconnectToRoomResponse response, CancellationToken cT);
    Task ReceiveGroup_ChangedPlayerOnline(ChangedPlayerOnlineResponse response, CancellationToken cT);
    Task ReceiveUser_ChangedPlayerOnline(ChangedPlayerOnlineResponse response, CancellationToken cT);
    Task ReceiveUser_ReconnectingInitRoomData(ReconnectingInitRoomDataResponse response, CancellationToken cT);
    Task ReceiveUser_Navigate(NavigateResponse response, CancellationToken cT);
    Task ReceiveGroup_AddedGameHistoryMessage(AddedGameHistoryMessageResponse response, CancellationToken cT);
    Task ReceiveGroup_ChangedRoomStatus(ChangedRoomStatusResponse response, CancellationToken cT);
}