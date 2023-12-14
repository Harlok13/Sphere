using App.Domain.Enums;
using App.Domain.Primitives;

namespace App.Domain.Entities;

public sealed class Room : Entity
{
    private readonly List<Player> _players = new();
    private const int DefaultPlayersInRoom = 1;
    
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
        Status = RoomStatus.Waiting;
        PlayersInRoom = DefaultPlayersInRoom;
    }

    public int RoomSize { get; private init; }

    public string RoomName { get; private set; }

    public int StartBid { get; private init; }

    public int MinBid { get; private init; }

    public int MaxBid { get; private init; }

    public string AvatarUrl { get; private set; }

    public int PlayersInRoom { get; private set; }

    public RoomStatus Status { get; private set; }

    // public IReadOnlyCollection<Player> Players => _players;
    public ICollection<Player> Players { get; set; }
    
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

    public void AddNewPlayer(Player player)  // TODO: validation
    {
        player.SetRoomId(Id);
        Players.Add(player);  // TODO: lock
        PlayersInRoom = Players.Count;
    }
}
