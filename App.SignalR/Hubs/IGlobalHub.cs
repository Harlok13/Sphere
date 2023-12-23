using App.Contracts.Data;
using App.Contracts.Responses;
using App.Contracts.Responses.PlayerResponses;
using App.Contracts.Responses.RoomResponses;

namespace App.SignalR.Hubs;

public interface IGlobalHub
{
    Task ReceiveAll_CreatedRoom(CreatedRoomResponse response, CancellationToken cT);
    Task ReceiveAll_UpdatedRoom(RoomInLobbyDto dto, CancellationToken cT);  
    Task ReceiveGroup_NewPlayer(PlayerResponse response, CancellationToken cT);
    Task ReceiveOwn_PlayerData(PlayerResponse playerResponse, InitRoomDataResponse initRoomDataResponse, IEnumerable<PlayerResponse> playersResponse, CancellationToken cT);
    Task ReceiveOwn_RemoveFromRoom(CancellationToken cT);
    Task ReceiveGroup_RemovedPlayer(RemovedPlayerResponse response, CancellationToken cT);
    Task ReceiveGroup_NewRoomLeader(PlayerResponse response, CancellationToken cT);
    Task ReceiveAll_RemovedRoom(Guid roomId, CancellationToken cT);
    Task ReceiveGroup_ChangedPlayerReadiness(ChangedPlayerReadinessResponse response, CancellationToken cT);
    Task ReceiveOwn_ChangedPlayerReadiness(ChangedPlayerReadinessResponse response, CancellationToken cT);
    Task ReceiveOwn_SelectStartGameMoney(SelectStartGameMoneyResponse response, CancellationToken cT);
    Task ReceiveOwn_NotEnoughMoneyNotification(NotEnoughMoneyNotificationResponse response, CancellationToken cT);
    Task ReceiveOwn_UpdatedMoney(int updatedMoney, CancellationToken cT);
    Task ReceiveGroup_NewRoomName(string roomRoomName, CancellationToken cT);
    Task ReceiveGroup_NewCard(CardDto dto, CancellationToken cT);
    Task ReceiveOwn_AddedCard(AddedCardResponse response, CancellationToken cT);
    Task ReceiveGroup_AddedCard(AddedCardResponse response, CancellationToken cT);
    Task ReceiveGroup_ChangedRoomBank(ChangedRoomBankResponse response, CancellationToken cT);
    Task ReceiveOwn_ChangedPlayerMove(ChangedPlayerMoveResponse response, CancellationToken cT);
    Task ReceiveMoveEnd(Guid playerId, CancellationToken cT);
    Task ReceiveGroup_StartGameErrorNotification(string? errorMsg, CancellationToken cT);
    Task ReceiveOwn_ChangedPlayerMoney(ChangedPlayerMoneyResponse response, CancellationToken cT);
    Task ReceiveGroup_ChangedPlayerMoney(ChangedPlayerMoneyResponse response, CancellationToken cT);
    Task ReceiveGroup_StartGame(CancellationToken cT);
    Task ReceiveOwn_UpdatedTimer(int seconds, CancellationToken cT);
    Task ReceiveOwn_Fold(CancellationToken cT);
    Task ReceiveGroup_Fold(Guid playerId, CancellationToken cT);
    Task ReceiveOwn_ChangedPlayerInGame(ChangedPlayerInGameResponse response, CancellationToken cT);
    Task ReceiveGroup_ChangedPlayerInGame(ChangedPlayerInGameResponse response, CancellationToken cT);
    Task ReceiveAll_UpdatedRoomStatus(UpdatedRoomStatusResponse response, CancellationToken cT);
    Task ReceiveAll_UpdatedRoomPlayersInRoom(UpdatedRoomPlayersInRoomResponse response, CancellationToken cT);
    Task ReceiveOwn_UpdatedPlayerIsLeader(UpdatedPlayerIsLeaderResponse response, CancellationToken cT);
    Task ReceiveGroup_UpdatedPlayerIsLeader(UpdatedPlayerIsLeaderResponse response, CancellationToken cT);
    Task ReceiveAll_ChangedRoomRoomName(ChangedRoomRoomNameResponse response, CancellationToken cT);
    Task ReceiveGroup_ChangedRoomRoomName(ChangedRoomRoomNameResponse response, CancellationToken cT);
    Task ReceiveAll_ChangedRoomAvatarUrl(ChangedRoomAvatarUrlResponse response, CancellationToken cT);
}