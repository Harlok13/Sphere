using System.Collections.Immutable;
using App.Domain.DomainEvents.RoomEvents;
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

    public ICollection<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();

    private Room(
        Guid id,
        string roomName,
        int roomSize,
        int startBid,
        int minBid,
        int maxBid,
        string avatarUrl
    ) : base(id)
    {
        RoomName = roomName;
        RoomSize = roomSize;
        StartBid = startBid;
        MinBid = minBid;
        MaxBid = maxBid;
        AvatarUrl = avatarUrl;
        Status = DefaultRoomStatus;
        PlayersInRoom = DefaultPlayersInRoom;
        Bank = default;
        
        DomainEvents.Add(new CreateRoomEvent(this));
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

    public IReadOnlyCollection<Player> Players => _players;

    // TODO: add created at (for GetFirstPageAsync)

    public static Room Create(
        Guid id,
        string roomName,
        int roomSize,
        int startBid,
        int minBid,
        int maxBid,
        string avatarUrl)
    {
        var room = new Room(
            id: id,
            roomName: roomName,
            roomSize: roomSize,
            startBid: startBid,
            minBid: minBid,
            maxBid: maxBid,
            avatarUrl: avatarUrl);
        
        return room;
    }

    public void AddNewPlayer(Player player) // TODO: validation
    {
        player.SetRoomId(Id);
        lock (_lock)
        {
            _players.Add(player);
            PlayersInRoom = _players.Count;

            if (PlayersInRoom == RoomSize)
            {
                Status = RoomStatus.Full;
            }
        }
    }

    public void RemovePlayer(Guid playerId)
    {
        lock (_lock)
        {
            var player = _players.Single(p => p.Id == playerId);
            _players.Remove(player);
            PlayersInRoom = Players.Count;
        }
    }

    public Player SetNewRoomLeader(Guid? playerId = null)
    {
        lock (_lock)
        {
            var newLeader = playerId is null
                ? _players.FirstOrDefault()
                : _players.Single(p => p.Id == playerId);

            newLeader.SetIsLeader(true);
            RoomName = $"{newLeader.PlayerName}'s Room";
            AvatarUrl = newLeader.AvatarUrl;

            return newLeader;
        }
    }

    public record BankEventArgs(int Value);

    public delegate Task BankHandlerAsync(Room sender, BankEventArgs e, CancellationToken cT);

    public event BankHandlerAsync? Notify;

    public async Task IncreaseBank(int value, CancellationToken cT)
    {
        Bank += value;
        await Notify?.Invoke(this, new BankEventArgs(Bank), cT)!;
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
                Status = RoomStatus.Playing;

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