using App.Contracts.Enums;

namespace App.Contracts.Requests;

public record SelectStartGameMoneyRequest(
    RoomRequest? RoomRequest,
    Guid PlayerId,
    Guid? RoomId);
    