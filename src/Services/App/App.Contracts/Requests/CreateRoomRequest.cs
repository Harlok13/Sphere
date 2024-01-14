namespace App.Contracts.Requests;

public sealed record CreateRoomRequest(
    RoomRequest RoomRequest,
    Guid PlayerId,
    int SelectedStartMoney,
    int UpperBound,
    int LowerBound);