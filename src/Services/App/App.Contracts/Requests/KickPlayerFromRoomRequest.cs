namespace App.Contracts.Requests;

public sealed record KickPlayerFromRoomRequest(
    Guid KickedPlayerId,
    Guid InitiatorId,
    Guid RoomId);