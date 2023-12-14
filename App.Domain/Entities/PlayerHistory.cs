using App.Domain.Enums;
using App.Domain.Primitives;
using App.Domain.ValueObjects;

namespace App.Domain.Entities;

public sealed class PlayerHistory : Entity
{
    private PlayerHistory(
        Guid id,
        string score,
        string cardsPlayed,
        GameResult result,
        Guid playerId
        ) : base(id)
    {
        Score = score;
        PlayedAt = DateTime.UtcNow;
        CardsPlayed = cardsPlayed;
        Result = result;
        PlayerId = playerId;
    }
    
    public Guid PlayerId { get; private init; }
    
    public string Score { get; private init; }

    public DateTime PlayedAt { get; private init; }

    public string CardsPlayed { get; private init; }

    public GameResult Result { get; private init; }
}