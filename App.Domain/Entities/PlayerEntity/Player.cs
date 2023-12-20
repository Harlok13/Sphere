using App.Domain.Entities.RoomEntity;
using App.Domain.Primitives;

namespace App.Domain.Entities.PlayerEntity;

public sealed partial class Player : Entity
{
    private readonly List<Card> _cards = new();

    private const string DefaultAvatarUrl = "img/avatars/default_avatar.png";

    private Player(
        Guid id,
        string playerName,
        Guid roomId,
        int money,
        string connectionId
    ) : base(id)
    {
        PlayerName = playerName;
        AvatarUrl = DefaultAvatarUrl; 
        RoomId = roomId;
        Money = money;
        ConnectionId = connectionId;
        IsLeader = default;
        Readiness = default;
        Score = default;
        Move = default;
        InGame = default;
    }

    public Guid RoomId { get; private set; }  

    public Room Room { get; private set; }  
    
    public string AvatarUrl { get; private set; }

    public bool IsLeader { get; private set; }

    public bool Readiness { get; private set; }

    public string PlayerName { get; private init; }

    public int Score { get; private set; }

    public IReadOnlyCollection<Card> Cards => _cards;

    public bool Move { get; private set; }

    public int Money { get; private set; }

    public string ConnectionId { get; private set; }  // TODO: init?

    public bool InGame { get; private set; }

    public static Player Create(
        Guid id,
        string playerName,
        Guid roomId,
        int money,
        string connectionId)
    {
        return new Player(
            id: id,
            playerName:
            playerName,
            roomId: roomId,
            money: money,
            connectionId: connectionId);
    }

    public void SetRoomId(Guid roomId)
    {
        RoomId = roomId;
    }

    public void SetIsLeader(bool value)
    {
        IsLeader = value;
    }

    public async Task SetIsLeaderNotifyAsync(bool value, CancellationToken cT)
    {
        SetIsLeader(value);
        await NotifyIsLeader?.Invoke(this, cT)!;
    }

    public void ToggleReadiness()
    {
        Readiness = !Readiness;
    }

    public async Task ToggleReadinessNotifyAsync(CancellationToken cT)
    {
        ToggleReadiness();
        await NotifyReadiness?.Invoke(this, cT)!;
    }

    public void DecreaseMoney(int value)
    {
        Money -= value;
    }

    public async Task DecreaseMoneyNotifyAsync(int value, CancellationToken cT)
    {
        DecreaseMoney(value);
        await NotifyMoney?.Invoke(this, cT)!;
    }

    public string SetMove(bool value)
    {
        Move = value;
        return ConnectionId;
    }

    public async Task SetMoveNotifyAsync(bool value, CancellationToken cT)
    {
        _ = SetMove(value);
        await NotifyMove?.Invoke(this, cT)!;
    }

    public void SetCard(Card card)
    {
        _cards.Add(card);
    }

    public async Task SetCardNotifyAsync(Card card, CancellationToken cT)
    {
        SetCard(card);
        await NotifyCard?.Invoke(this, new CardEventArgs(card), cT)!;
    }

    public void ResetCards()
    {
        _cards.Clear();
    }

    public async Task ResetCardsNotifyAsync(CancellationToken cT)
    {
        ResetCards();
        await NotifyCards?.Invoke(this, cT)!;
    }

    public void SetInGame(bool value)
    {
        InGame = value;
    }

    public async Task SetInGameNotifyAsync(bool value, CancellationToken cT)
    {
        SetInGame(value);
        await NotifyInGame?.Invoke(this, cT)!;
    }
}