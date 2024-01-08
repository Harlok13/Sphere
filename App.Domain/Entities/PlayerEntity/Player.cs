using System.ComponentModel;
using App.Domain.DomainEvents.PlayerDomainEvents;
using App.Domain.DomainResults;
using App.Domain.Entities.RoomEntity;
using App.Domain.Enums;
using App.Domain.Messages;
using App.Domain.Primitives;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace App.Domain.Entities.PlayerEntity;

public sealed partial class Player : Entity, IHasDomainEvent
{
    // private readonly List<Card> _cards = new();
    private List<Card>? _cards;

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

    // public IReadOnlyCollection<Card> Cards => _cards;

    [Description("Json column")]
    public string? Cards { get; private set; }

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

    internal void SetIsLeader(bool value)
    {
        IsLeader = value;
        _domainEvents.Add(new ChangedPlayerIsLeaderDomainEvent(
            IsLeader: IsLeader,
            RoomId: RoomId,
            PlayerId: Id));
    }

    internal void ToggleReadiness()
    {
        Readiness = !Readiness;
        _domainEvents.Add(new ChangedPlayerReadinessDomainEvent(
            RoomId: RoomId,
            PlayerId: Id,
            Readiness: Readiness,
            ConnectionId: ConnectionId));
    }

    private DomainResult DecreaseMoney(int value) 
    {
        if (Money < value) 
            return new DomainFailure(
                Message.Failure.Player.DecreaseMoney.NotEnoughMoney(Money - value));
        
        Money -= value;
        _domainEvents.Add(new ChangedPlayerMoneyDomainEvent(
            Money: Money,
            RoomId: RoomId,
            PlayerId: Id));

        return new DomainSuccessResult();
    }

    public void IncreaseMoney(int value)  // TODO: make private/internal
    {
        Money += value;
        _domainEvents.Add(new ChangedPlayerMoneyDomainEvent(
            Money: Money,
            RoomId: RoomId,
            PlayerId: Id));
    }

    internal void SetMove(bool value)
    {
        Move = value;
        _domainEvents.Add(new ChangedPlayerMoveDomainEvent(Move: Move, PlayerId: Id));
    }

    private DomainResult AddNewCard(Card card, int delayMs = default) 
    {
        var syncCardsResult = SyncCards();
        if (syncCardsResult is DomainError syncCardsError) return syncCardsError;
        
        _cards!.Add(card);
        SetScore(card.Value);

        Cards = JsonSerializer.Serialize(_cards);
        
        _domainEvents.Add(new AddedCardDomainEvent(
            Card: card,
            DelayMs: delayMs,
            RoomId: RoomId,
            PlayerId: Id));

        return new DomainSuccessResult();
    }

    private void SetScore(int cardValue)
    {
        Score = cardValue;
    }

    internal void ResetCards()
    {
        _cards?.Clear();
        Cards = default;
    }

    private void SetInGame(bool value)  
    {
        InGame = value;
        _domainEvents.Add(new ChangedPlayerInGameDomainEvent(
            RoomId: RoomId,
            PlayerId: Id,
            InGame: InGame));
    }

    public void SetMoveStatus(MoveStatus moveStatus)  // TODO: make private/internal
    {
        MoveStatus = moveStatus;
    }

    private void SetConnectionId(string connId)
    {
        ConnectionId = connId;
    }

    internal void SetOnline(bool onlineValue, string? connId = null)
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

    internal DomainResult GetCardsCount()
    {
        var syncCardsResult = SyncCards();
        if (syncCardsResult is DomainError syncCardsError) return syncCardsError;

        return new DomainSuccessResult<int>(_cards!.Count);
    }

    private DomainResult SyncCards()
    {
        if (Cards is null) _cards ??= new List<Card>();

        // try
        // {
        _cards ??= JsonSerializer.Deserialize<List<Card>>(Cards!);
        // }
        // catch ()
        if (_cards is null)
            return new DomainError(
                Message.Error.DeserializeError(nameof(_cards), nameof(Room)));

        return new DomainSuccessResult();
    }
}