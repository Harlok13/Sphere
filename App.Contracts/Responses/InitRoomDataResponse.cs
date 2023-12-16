namespace App.Contracts.Responses;

public sealed record InitRoomDataResponse(
    Guid Id,
    int RoomSize,
    string RoomName,
    int StartBid,
    int MinBid,
    int MaxBid);
