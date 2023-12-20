using App.Domain.Entities;

namespace App.Contracts.Responses;

public sealed record PlayerResponse(
    Guid Id,
    Guid? RoomId,
    bool IsLeader,
    bool Readiness,
    string PlayerName,
    int Score,
    string AvatarUrl,
    IEnumerable<Card> Cards,
    bool Move,
    int Money,
    bool InGame);