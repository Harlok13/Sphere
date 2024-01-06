using System.ComponentModel;
using System.Text.Json;
using App.Domain.DomainEvents.RoomDomainEvents;
using App.Domain.DomainResults;
using App.Domain.Entities.PlayerEntity;
using App.Domain.Enums;
using App.Domain.Messages;
using App.Domain.Primitives;

namespace App.Domain.Entities.RoomEntity;

public sealed partial class Room : AggregateRoot, IHasDomainEvent
{
    private readonly List<Player> _players = new();
    private readonly List<KickedPlayer> _kickedPlayers = new();
    private List<CardInDeck>? _cardsDeck;

    private const int DefaultPlayersInRoom = 1;
    private const RoomStatus DefaultRoomStatus = RoomStatus.Waiting;
    private const int MaxCardsCount = 6;
    
    private readonly object _lock = new();

    private readonly List<DomainEvent> _domainEvents = new(); 
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents;
    
    public Player this[int index]
    {
        get { lock(_players) return _players[index]; }
    }

    private Room(
        Guid id,
        string roomName,
        int roomSize,
        int startBid,
        int minBid,
        int maxBid,
        string avatarUrl,
        int upperStartMoneyBound,
        int lowerStartMoneyBound
    ) : base(id)
    {
        RoomName = roomName;
        RoomSize = roomSize;
        StartBid = startBid;
        MinBid = minBid;
        MaxBid = maxBid;
        AvatarUrl = avatarUrl;
        UpperStartMoneyBound = upperStartMoneyBound;
        LowerStartMoneyBound = lowerStartMoneyBound;
        Status = DefaultRoomStatus;
        PlayersInRoom = DefaultPlayersInRoom;
        Bank = default;
    }

    public int RoomSize { get; private init; }

    public string RoomName { get; private set; }

    public int StartBid { get; private init; }

    public int MinBid { get; private init; }

    public int MaxBid { get; private init; }

    public string AvatarUrl { get; private set; }

    public int PlayersInRoom { get; private set; }

    public RoomStatus Status { get; private set; }

    public int Bank { get; private set; }

    public int LowerStartMoneyBound { get; private init; }

    public int UpperStartMoneyBound { get; private init; }

    public IReadOnlyCollection<Player> Players => _players;

    public IReadOnlyCollection<KickedPlayer> KickedPlayers => _kickedPlayers;

    [Description("Json column")]
    public string? CardsDeck { get; private set; }
    
    [Description("Json column")]
    public string? GameHistory { get; private set; }

    // TODO: add created at (for GetFirstPageAsync)

    public static Room Create(
        Guid id,
        string roomName,
        int roomSize,
        int startBid,
        int minBid,
        int maxBid,
        string avatarUrl,
        int lowerStartMoneyBound,
        int upperStartMoneyBound)
    {
        var room = new Room(
            id: id,
            roomName: roomName,
            roomSize: roomSize,
            startBid: startBid,
            minBid: minBid,
            maxBid: maxBid,
            avatarUrl: avatarUrl,
            lowerStartMoneyBound: lowerStartMoneyBound,
            upperStartMoneyBound: upperStartMoneyBound);
        
        room._domainEvents.Add(new CreatedRoomDomainEvent(room));
        return room;
    }

    private void AddNewPlayer(Player player) // TODO: validation
    {
        lock (_players)
        {
            _players.Add(player);
            SetPlayersInRoom(_players.Count);
            
            _domainEvents.Add(new AddedPlayerDomainEvent(player, this));

            if (PlayersInRoom == RoomSize)
            {
                SetRoomStatus(RoomStatus.Full);
            }
        }
    }

    private DomainResult RemovePlayer(Guid playerId)
    {
        lock (_players)
        {
            var player = _players.SingleOrDefault(p => p.Id == playerId);
            if (player is null)
                return new DomainError(
                    Message.Error.Room.PlayerNotFound(nameof(RemovePlayer), playerId));
            
            _players.Remove(player);
            SetPlayersInRoom(Players.Count);
            
            _domainEvents.Add(new RemovedPlayerDomainEvent(
                RoomId: Id,
                PlayerId: player.Id,
                ConnectionId: player.ConnectionId,
                PlayersInRoom: PlayersInRoom));

            return new DomainSuccessResult();
        }
    }

    private void RemovePlayer(Player player)
    {
        lock (_players)
        {
            _players.Remove(player);
            SetPlayersInRoom(Players.Count);
            
            _domainEvents.Add(new RemovedPlayerDomainEvent(
                RoomId: Id,
                PlayerId: player.Id,
                ConnectionId: player.ConnectionId,
                PlayersInRoom: PlayersInRoom));
        }
    }

    private void SetPlayersInRoom(int playersCount)
    {
        PlayersInRoom = playersCount;

        if (PlayersInRoom == 0)
        {
            _domainEvents.Add(new RemovedRoomDomainEvent(Id));
            return;
        }
        _domainEvents.Add(new ChangedRoomPlayersInRoomDomainEvent(RoomId: Id, PlayersInRoom: PlayersInRoom));
    }

    private void SetRoomStatus(RoomStatus status)
    {
        Status = status;
        _domainEvents.Add(new ChangedRoomStatusDomainEvent(RoomId: Id, RoomStatus: Status));
    }

    private void SetAvatarUrl(string avatarUrl)
    {
        AvatarUrl = avatarUrl;
        _domainEvents.Add(new ChangedRoomAvatarUrlDomainEvent(RoomId: Id, AvatarUrl: AvatarUrl));
    }

    private void SetRoomName(string name, bool withPostfix = false)
    {
        RoomName = withPostfix
            ? $"{name}'s Room"
            : name;
        
        _domainEvents.Add(new ChangedRoomRoomNameDomainEvent(RoomId: Id, RoomName: RoomName));
    }

    private void IncreaseBank(int value)  
    {
        Bank += value;
        _domainEvents.Add(new ChangedRoomBankDomainEvent(RoomId: Id, Bank: Bank));
    }

    public void ResetBank()  // TODO: make private
    {
        Bank = default;
        _domainEvents.Add(new ChangedRoomBankDomainEvent(RoomId: Id, Bank: Bank));
    }


    private void AddKickedPlayer(Player initiator, Player kickedPlayer)
    {
        lock (_kickedPlayers)
        {
            var kP = KickedPlayer.Create(
                id: Guid.NewGuid(),
                playerId: kickedPlayer.Id,
                playerName: kickedPlayer.PlayerName,
                whoKickId: initiator.Id,
                whoKickName: initiator.PlayerName,
                roomId: Id,
                room: this);
            _kickedPlayers.Add(kP);
            
            _domainEvents.Add(new AddedKickedPlayerDomainEvent(
                InitiatorConnectionId: initiator.ConnectionId,
                KickedPlayerConnectionId: kickedPlayer.ConnectionId,
                NotificationForInitiator: Message.Notification.Room.AddKickedPlayer.SuccessKick(kickedPlayer.PlayerName),
                NotificationForKickedPlayer: Message.Notification.Room.AddKickedPlayer.WasKicked(initiator.PlayerName)));
        }
    }

    private void UpdateCardsDeck(IEnumerable<CardInDeck>? cardInDecks)  // TODO: ref
    {
        if (cardInDecks is not null) _cardsDeck ??= cardInDecks.ToList();
        CardsDeck = JsonSerializer.Serialize(_cardsDeck);
    }

    private DomainResult GetCardsDeck()  
    {
        if (CardsDeck is null)
            return new DomainError(
                Message.Error.FieldIsNull(nameof(CardsDeck), nameof(Room)));
        
        _cardsDeck ??= JsonSerializer.Deserialize<List<CardInDeck>>(CardsDeck);
        if (_cardsDeck is null)
            return new DomainError(
                Message.Error.DeserializeError(nameof(_cardsDeck), nameof(Room)));

        return new DomainSuccessResult<List<CardInDeck>>(_cardsDeck);
    }
}
