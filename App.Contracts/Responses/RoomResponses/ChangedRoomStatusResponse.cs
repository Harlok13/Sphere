using App.Domain.Enums;

namespace App.Contracts.Responses.RoomResponses;

public sealed record ChangedRoomStatusResponse(
    Guid RoomId,
    RoomStatus RoomStatus);