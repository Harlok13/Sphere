using App.Contracts.Responses;

namespace App.SignalR.Hubs;

public interface IGlobalHub
{
    Task ReceiveAll_NewRoom(RoomInLobbyResponse roomInLobbyResponse, CancellationToken cT);

    Task ReceiveAll_UpdatedRoom(RoomInLobbyResponse roomInLobbyResponse, CancellationToken cT);  // TODO: change to property and id only
    
    Task ReceiveGroup_NewPlayer(PlayerResponse playerResponse, CancellationToken cT);

    Task ReceiveOwn_PlayerData(PlayerResponse playerResponse, InitRoomDataResponse initRoomDataResponse, IEnumerable<PlayerResponse> playerResponses, CancellationToken cT);
    
    Task ReceiveOwn_RemoveFromRoom(CancellationToken cT);
    Task ReceiveGroup_RemovedPlayer(Guid playerId, CancellationToken cT);
    Task ReceiveGroup_NewRoomLeader(PlayerResponse newLeaderResponse, CancellationToken cT);
    Task ReceiveAll_RemovedRoom(Guid roomId, CancellationToken cT);
    Task ReceiveGroup_UpdatedPlayer(PlayerResponse playerResponse, CancellationToken cT);
    Task ReceiveOwn_Readiness(bool playerReadiness, CancellationToken cT);
    Task ReceiveOwn_SelectStartGameMoney(SelectStartGameMoneyResponse selector, CancellationToken cT);
    Task ReceiveOwn_NotEnoughMoneyNotification(NotificationResponse notificationResponse, CancellationToken cT);
    Task ReceiveOwn_UpdatedMoney(int updatedMoney, CancellationToken cT);
    Task ReceiveGroup_NewRoomName(string roomRoomName, CancellationToken cT);
    Task ReceiveGroup_NewCard(CardResponse cardResponse, CancellationToken cT);
    Task ReceiveOwn_Card(CardResponse cardResponse, CancellationToken cT);
    Task ReceiveGroup_UpdatedBank(int bankValue, CancellationToken cT);
    Task ReceiveOwn_Move(CancellationToken cT);
    Task ReceiveMoveEnd(Guid playerId, CancellationToken cT);
    Task ReceiveGroup_StartGameErrorNotification(string? errorMsg, CancellationToken cT);
    Task ReceiveOwn_UpdatedGameMoney(int playerMoney, CancellationToken cT);
    Task ReceiveGroup_StartGame(CancellationToken cT);
    Task ReceiveOwn_UpdatedTimer(int seconds, CancellationToken cT);
    Task ReceiveOwn_FoldCards(CancellationToken cT);
    Task ReceiveGroup_PlayerFold(Guid playerId, CancellationToken cT);
    Task ReceiveOwn_InGame(bool inGame, CancellationToken cT);
    Task ReceiveGroup_PlayerInGame(Guid playerId, bool inGame, CancellationToken cT);  // TODO: redundant?
    
}