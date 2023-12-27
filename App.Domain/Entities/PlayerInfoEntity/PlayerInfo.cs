using App.Domain.DomainEvents.PlayerInfoDomainEvents;
using App.Domain.Primitives;

namespace App.Domain.Entities.PlayerInfoEntity;

public sealed partial class PlayerInfo : Entity, IHasDomainEvent
{
    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents;

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
        HasGold21 = default;
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
    
    public int HasGold21 { get; private set; }
    
    // TODO: max money win prop
    

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
        _domainEvents.Add(new ChangedPlayerInfoMoneyDomainEvent(Money, UserId));
    }

    public void DecrementMoney(int value)
    {
        Money -= value;  // TODO: validation
        _domainEvents.Add(new ChangedPlayerInfoMoneyDomainEvent(Money, UserId));
    }

    private void IncreaseMatches() => Matches += 1;
    private void IncreaseWins() => Wins += 1;
    private void IncreaseLoses() => Loses += 1;
    private void IncreaseDraws() => Draws += 1;
    private void IncreaseHas21() => Has21 += 1;
    private void IncreaseHasGold21() => HasGold21 += 1;

    public void Win(bool has21 = default, bool hasGold21 = default)
    {
        IncreaseMatches();
        IncreaseWins();
        if (has21) IncreaseHas21();
        if (hasGold21) IncreaseHas21();
    }

    public void Lose()
    {
        IncreaseMatches();
        IncreaseLoses();
    }

    public void Draw(bool has21 = default, bool hasGold21 = default)
    {
        IncreaseMatches();
        IncreaseDraws();
        if (has21) IncreaseHas21();
        if (hasGold21)
        {
            IncreaseHasGold21();
            // TODO: give achievements
        }
    }
}
