using App.Contracts.Enums;

namespace App.Contracts.Requests;

public sealed record RemoveFromRoomRequest(
    Guid RoomId,
    Guid PlayerId);