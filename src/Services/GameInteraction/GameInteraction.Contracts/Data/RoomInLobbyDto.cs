using GameInteraction.Domain.Enums;

namespace GameInteraction.Contracts.Data;

public sealed record RoomInLobbyDto(
    Guid Id,
    string RoomName,
    int RoomSize,
    int StartBid,
    int MinBid,
    int MaxBid,
    string AvatarUrl,
    ERoomStatus Status,
    int PlayersInRoom,
    int Bank,
    int LowerStartMoneyBound,
    int UpperStartMoneyBound);