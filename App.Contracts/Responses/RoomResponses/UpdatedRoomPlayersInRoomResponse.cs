namespace App.Contracts.Responses.RoomResponses;

public sealed record UpdatedRoomPlayersInRoomResponse(
    Guid RoomId,
    int PlayersInRoom);