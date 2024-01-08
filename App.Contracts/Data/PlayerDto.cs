using App.Domain.Entities;

namespace App.Contracts.Data;

public sealed record PlayerDto(
    Guid Id,
    Guid RoomId,
    bool IsLeader,
    bool Readiness,
    string PlayerName,
    int Score,
    string AvatarUrl,
    IEnumerable<Card> Cards,
    bool Move,
    int Money,
    bool InGame,
    bool Online);