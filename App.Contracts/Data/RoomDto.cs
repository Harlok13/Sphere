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
    RoomStatus Status,
    int PlayersInRoom,
    IEnumerable<PlayerDto> Players, 
    int Bank);