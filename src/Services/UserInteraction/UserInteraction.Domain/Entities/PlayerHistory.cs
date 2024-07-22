using System.ComponentModel;
using Core;
using UserInteraction.Domain.Enums;

namespace UserInteraction.Domain.Entities;

public sealed class PlayerHistory : Entity
{
    private PlayerHistory(
        Guid id,
        string score,
        string cardsPlayed,
        EGameResult result,
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

    [Description("jsonb field")]
    public string CardsPlayed { get; private init; }

    public EGameResult Result { get; private init; }
    
    // TODO: money win or lose count
}