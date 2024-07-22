using GameInteraction.Domain.Enums;

namespace GameInteraction.Contracts.Data;

public sealed record RoomDto(
    Guid Id,
    string RoomName,
    int RoomSize,
    int StartBid,
    int MinBid,
    int MaxBid,
    string AvatarUrl,
    int PlayersInRoom,
    ERoomStatus Status,
    int Bank,
    int LowerStartMoneyBound,
    int UpperStartMoneyBound,
    IEnumerable<PlayerDto> Players);
