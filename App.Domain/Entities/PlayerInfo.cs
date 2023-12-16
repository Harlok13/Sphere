using App.Domain.Primitives;

namespace App.Domain.Entities;

public sealed class PlayerInfo : Entity   
{
    private PlayerInfo(
        Guid id,
        Guid userId, 
        string playerName
    ) : base(id)
    {
        UserId = userId;
        AvatarUrl = "img/avatars/default_avatar.png";
        PlayerName = playerName;
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

    public Guid UserId { get; private init; }
    
    public string AvatarUrl { get; private set; }

    public string PlayerName { get; private set; }
    
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

    public static PlayerInfo Create(
        Guid id,
        Guid userId,
        string playerName)
    {
        return new PlayerInfo(id, userId, playerName);
    }
    
    public void IncrementMoney(int value)
    {
        Money += value;
    }

    public void DecrementMoney(int value)
    {
        Money -= value;  // TODO: validation
    }
}
