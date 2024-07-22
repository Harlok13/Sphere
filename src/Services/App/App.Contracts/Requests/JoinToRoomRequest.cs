namespace App.Contracts.Requests;

public sealed record JoinToRoomRequest(
    Guid RoomId,
    Guid PlayerId,
    int SelectedStartMoney);