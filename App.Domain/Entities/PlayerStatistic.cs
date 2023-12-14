using App.Domain.Primitives;

namespace App.Domain.Entities;

public sealed class PlayerStatistic : Entity
{
    internal PlayerStatistic(
        Guid id,
        Guid playerId
    ) : base(id)
    {
        PlayerId = playerId;
        Matches = default;
        Loses = default;
        Wins = default;
        Draws = default;
        AllExp = default;
        CurrentExp = default;
        TargetExp = default;
        Money = default;
        Likes = default;
        Level = default;
        Has21 = default;
    }

    public Guid PlayerId { get; private init; }
    
    public int Matches { get; private set; }

    public int Loses { get; private set; }

    public int Wins { get; private set; }

    public int Draws { get; private set; }

    public int AllExp { get; private set; }
    
    public int CurrentExp { get; private set; }
    
    public int TargetExp { get; private set; }
    
    public int Money { get; private set; }

    public int Likes { get; private set; }

    public int Level { get; private set; }

    public int Has21 { get; private set; }
    
    // public UserStatisticModel(int id) => Id = id;

    public static PlayerStatistic Create(
        Guid id,
        Guid playerId)
    {
        return new PlayerStatistic(id, playerId);
    }
}
