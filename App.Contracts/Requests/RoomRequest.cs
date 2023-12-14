namespace App.Contracts.Requests;

public sealed record RoomRequest(
    string RoomName,
    int RoomSize,
    int StartBid,
    int MinBid,
    int MaxBid,
    string ImgUrl,
    string Status,
    int PlayersInRoom);