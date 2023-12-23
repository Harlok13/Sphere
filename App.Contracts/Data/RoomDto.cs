namespace App.Contracts.Data;

public sealed record RoomDto(
    string RoomName,
    int RoomSize,
    int StartBid,
    int MinBid,
    int MaxBid,
    string AvatarUrl,
    string Status,
    int PlayersInRoom);