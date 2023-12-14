using App.Domain.Enums;

namespace App.Contracts.Responses;

public sealed record RoomInLobbyResponse
(
    Guid Id,
    string RoomName,
    int RoomSize,
    int StartBid,
    int MinBid,
    int MaxBid,
    string ImgUrl,
    RoomStatus Status,
    int PlayersInRoom);