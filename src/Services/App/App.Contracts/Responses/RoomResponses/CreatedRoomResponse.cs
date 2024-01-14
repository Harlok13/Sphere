using App.Contracts.Data;

namespace App.Contracts.Responses.RoomResponses;

public sealed record CreatedRoomResponse(
    RoomInLobbyDto RoomInLobbyDto);