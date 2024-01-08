namespace App.Contracts.Data;

public sealed record InitRoomDataDto(
    Guid Id,
    int RoomSize,
    string RoomName,
    int StartBid,
    int MinBid,
    int MaxBid);
