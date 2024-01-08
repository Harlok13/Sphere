using App.Contracts.Responses;
using App.Domain.Enums;

namespace App.Contracts.Data;

public sealed record RoomDto(
    Guid Id,
    string RoomName,
    int RoomSize,
    int StartBid,
    int MinBid,
    int MaxBid,
    string AvatarUrl,
    int PlayersInRoom,
    RoomStatus Status,
    int Bank,
    int LowerStartMoneyBound,
    int UpperStartMoneyBound,
    IEnumerable<PlayerDto> Players);
