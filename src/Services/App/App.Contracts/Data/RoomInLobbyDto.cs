using App.Domain.Enums;

namespace App.Contracts.Data;

public sealed record RoomInLobbyDto(
    Guid Id,
    string RoomName,
    int RoomSize,
    int StartBid,
    int MinBid,
    int MaxBid,
    string AvatarUrl,
    RoomStatus Status,
    int PlayersInRoom,
    int Bank,
    int LowerStartMoneyBound,
    int UpperStartMoneyBound);