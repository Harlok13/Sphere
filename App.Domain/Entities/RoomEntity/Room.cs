using System.Collections.Immutable;
using App.Domain.DomainEvents.RoomDomainEvents;
using App.Domain.Entities.PlayerEntity;
using App.Domain.Enums;
using App.Domain.Primitives;

namespace App.Domain.Entities.RoomEntity;

public sealed partial class Room : Entity, IHasDomainEvent
{
    private readonly List<Player> _players = new();

    private const int DefaultPlayersInRoom = 1;
    private const RoomStatus DefaultRoomStatus = RoomStatus.Waiting;
    
    private readonly object _lock = new();

    private readonly List<DomainEvent> _domainEvents = new(); 
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents;
    
    public Player this[int index]
    {
        get { lock(_lock) return _players[index]; }
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

    public void AddNewPlayer(Player player) // TODO: validation
    {
        lock (_lock)
        {
            _players.Add(player);
            _domainEvents.Add(new AddedPlayerDomainEvent(player, this));
            SetPlayersInRoom(_players.Count);

            if (PlayersInRoom == RoomSize)
            {
                SetRoomStatus(RoomStatus.Full);
            }
        }
    }

    internal void SetPlayersInRoom(int playerCount)
    {
        PlayersInRoom = playerCount;
        _domainEvents.Add(new ChangedRoomPlayersInRoomDomainEvent(RoomId: Id, PlayersInRoom: PlayersInRoom));
    }

    internal void SetRoomStatus(RoomStatus status)
    {
        Status = status;
        _domainEvents.Add(new ChangedRoomStatusDomainEvent(RoomId: Id, RoomStatus: Status));
    }

    public void RemovePlayer(Guid playerId, string connectionId)
    {
        lock (_lock)
        {
            var player = _players.Single(p => p.Id == playerId);
            _players.Remove(player);
            SetPlayersInRoom(Players.Count);
            
            _domainEvents.Add(new RemovedPlayerDomainEvent(
                RoomId: Id, PlayerId: playerId, ConnectionId: connectionId, PlayersInRoom: PlayersInRoom));
        }
    }

    public void SetNewRoomLeader(Guid? playerId = null)
    {
        lock (_lock)
        {
            var newLeader = playerId is null
                ? _players.FirstOrDefault()
                : _players.Single(p => p.Id == playerId);

            newLeader.SetIsLeader(value: true, playersInRoom: PlayersInRoom);
            SetRoomName(name: newLeader.PlayerName, withPostfix: true);
            SetAvatarUrl(newLeader.AvatarUrl);
        }
    }

    internal void SetAvatarUrl(string avatarUrl)
    {
        AvatarUrl = avatarUrl;
        _domainEvents.Add(new ChangedRoomAvatarUrlDomainEvent(RoomId: Id, AvatarUrl: AvatarUrl));
    }
    
    internal void SetRoomName(string name, bool withPostfix = false)
    {
        RoomName = withPostfix
            ? $"{name}'s Room"
            : name;
        
        _domainEvents.Add(new ChangedRoomRoomNameDomainEvent(RoomId: Id, RoomName: RoomName));
    }

    public void IncreaseBank(int value)
    {
        Bank += value;
        _domainEvents.Add(new ChangedRoomBankDomainEvent(RoomId: Id, Bank: Bank));
    }

    public record StartGameResult(bool CanStart, string? MovePlayerConnectionId, string? ErrorMsg);

    public StartGameResult StartGame(Guid leaderId)
    {
        lock (_lock)
        {
            var roomLeader = _players.Single(p => p.Id == leaderId);
            var canStartGame = PlayersInRoom > 1
                               && roomLeader is { IsLeader: true, Readiness: true }
                               && Players.All(p => p.Readiness);


            if (canStartGame)
            {
                SetRoomStatus(RoomStatus.Playing);

                // foreach (var player in Players)
                // {
                //     player.DecreaseMoney(StartBid);
                //     IncreaseBank(StartBid);
                // }

                var index = new Random().Next(_players.Count);
                var movePlayerConnectionId = _players.ToImmutableArray()[index].SetMove(true);


                return new StartGameResult(CanStart: true, MovePlayerConnectionId: movePlayerConnectionId,
                    ErrorMsg: null);
            }

            const string errorMsg = ""; // TODO: finish
            return new StartGameResult(CanStart: false, MovePlayerConnectionId: default, ErrorMsg: errorMsg);
        }
    }
}
