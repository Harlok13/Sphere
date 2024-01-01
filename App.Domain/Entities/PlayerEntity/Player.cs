using App.Domain.DomainEvents.PlayerDomainEvents;
using App.Domain.Entities.RoomEntity;
using App.Domain.Enums;
using App.Domain.Primitives;

namespace App.Domain.Entities.PlayerEntity;

public sealed partial class Player : Entity, IHasDomainEvent
{
    private readonly List<Card> _cards = new();

    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents;

    private const string DefaultAvatarUrl = "img/avatars/default_avatar.png";
    private const bool DefaultOnline = true;

    private Player(
        Guid id,
        string playerName,
        Guid roomId,
        int money,
        string connectionId,
        bool isLeader
    ) : base(id)
    {
        PlayerName = playerName;
        RoomId = roomId;
        Money = money;
        ConnectionId = connectionId;
        IsLeader = isLeader;
        AvatarUrl = DefaultAvatarUrl;
        Online = DefaultOnline;
        Readiness = default;
        Score = default;
        Move = default;
        InGame = default;
        MoveStatus = default;
    }

    public Guid RoomId { get; private init; }  

    public Room Room { get; private init; }  
    
    public string AvatarUrl { get; private init; }

    public bool IsLeader { get; private set; }

    public bool Readiness { get; private set; }

    public string PlayerName { get; private init; }

    public int Score { get; private set; }

    public IReadOnlyCollection<Card> Cards => _cards;

    public bool Move { get; private set; }

    public int Money { get; private set; }

    public string ConnectionId { get; private set; } 

    public bool InGame { get; private set; }

    public MoveStatus MoveStatus { get; private set; }

    public bool Online { get; private set; }

    internal static Player Create(
        Guid id,
        string playerName,
        Guid roomId,
        int money,
        string connectionId,
        bool isLeader,
        Room room)
    {
        var player = new Player(
            id: id,
            playerName: playerName,
            roomId: roomId,
            money: money,
            connectionId: connectionId,
            isLeader: isLeader)
        {
            Room = room
        };

        player._domainEvents.Add(new CreatedPlayerDomainEvent(player, room));
        return player;
    }

    public void SetIsLeader(bool value)
    {
        IsLeader = value;
        _domainEvents.Add(new ChangedPlayerIsLeaderDomainEvent(
            IsLeader: IsLeader,
            ConnectionId: ConnectionId,
            RoomId: RoomId,
            PlayerId: Id));
    }

    public void ToggleReadiness()
    {
        Readiness = !Readiness;
        _domainEvents.Add(new ChangedPlayerReadinessDomainEvent(
            RoomId: RoomId,
            PlayerId: Id,
            Readiness: Readiness,
            ConnectionId: ConnectionId));
    }

    public void DecreaseMoney(int value)
    {
        Money -= value;
        _domainEvents.Add(new ChangedPlayerMoneyDomainEvent(
            Money: Money,
            ConnectionId: ConnectionId,
            RoomId: RoomId,
            PlayerId: Id));
    }

    public void IncreaseMoney(int value)
    {
        Money += value;
        _domainEvents.Add(new ChangedPlayerMoneyDomainEvent(
            Money: Money,
            ConnectionId: ConnectionId,
            RoomId: RoomId,
            PlayerId: Id));
    }

    public string SetMove(bool value)
    {
        Move = value;
        _domainEvents.Add(new ChangedPlayerMoveDomainEvent(Move: Move, ConnectionId: ConnectionId));
        return ConnectionId;
    }
    
    public void AddNewCard(Card card, int delayMs)
    {
        _cards.Add(card);
        SetScore(card.Value);
        _domainEvents.Add(new AddedCardDomainEvent(
            Card: card,
            DelayMs: delayMs,
            RoomId: RoomId,
            PlayerId: Id,
            ConnectionId: ConnectionId));
    }

    private void SetScore(int cardValue)
    {
        Score = cardValue;
    }

    public void ResetCards()
    {
        _cards.Clear();
    }

    public void SetInGame(bool value)
    {
        InGame = value;
        _domainEvents.Add(new ChangedPlayerInGameDomainEvent(
            RoomId: RoomId,
            PlayerId: Id,
            InGame: InGame,
            ConnectionId: ConnectionId));
    }

    public void SetMoveStatus(MoveStatus moveStatus)
    {
        MoveStatus = moveStatus;
    }

    public void Hit()
    {
        SetMoveStatus(MoveStatus.Hit);
        SetMove(false);
    }

    public void Stay()
    {
        SetMoveStatus(MoveStatus.Stay);
        SetMove(false);
    }

    public void EndGame()
    {
        ResetCards();
        SetScore(default);
        ToggleReadiness();
        SetInGame(default);
        SetMoveStatus(MoveStatus.None);
        
        // TODO: check money, suggest setting a new amount
    }

    private void SetConnectionId(string connId)
    {
        ConnectionId = connId;
    }

    public void SetOnline(bool onlineValue, string? connId = null)
    {
        Online = onlineValue;

        if (onlineValue && connId is null) throw new Exception("ConnectionId can't be null.");  // TODO: custom ex
        
        connId ??= String.Empty;
        SetConnectionId(connId);

        _domainEvents.Add(new ChangedPlayerOnlineDomainEvent(
            Online: Online,
            RoomId: RoomId,
            PlayerId: Id,
            ConnectionId: ConnectionId));
    }
}