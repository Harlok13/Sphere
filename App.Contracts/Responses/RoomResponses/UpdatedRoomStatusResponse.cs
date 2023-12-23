using App.Domain.Enums;

namespace App.Contracts.Responses.RoomResponses;

public sealed record UpdatedRoomStatusResponse(
    Guid RoomId,
    RoomStatus RoomStatus);