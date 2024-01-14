namespace App.Contracts.Responses.RoomResponses;

public sealed record ChangedRoomPlayersInRoomResponse(
    Guid RoomId,
    int PlayersInRoom);