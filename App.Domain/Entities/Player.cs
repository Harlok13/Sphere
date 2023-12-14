using App.Domain.Primitives;

namespace App.Domain.Entities;

public sealed class Player : Entity
{
    private readonly List<Card> _cards = new();

    private Player(
        Guid id,
        string playerName,
        bool isLeader,
        Guid roomId
        // Guid roomId,
        // bool isLeader,
        // bool readiness,
        // string playerName,
        // int score
    ) : base(id)
    {
        PlayerName = playerName;
        IsLeader = isLeader;
        AvatarUrl = "img/avatars/default_avatar.png";  // TODO: hardcode
        RoomId = roomId;
        Readiness = false;
        Score = 0;
        Cards = null;
        // RoomId = roomId;
        // IsLeader = isLeader;
        // Readiness = readiness;
        // PlayerName = playerName;
        // Score = score;
    }

    public Guid RoomId { get; set; }

    public Room Room { get; set; }

    public string AvatarUrl { get; private set; }

    public bool IsLeader { get; private set; }

    public bool Readiness { get; private set; }

    public string PlayerName { get; private init; }

    public int Score { get; private set; }

    // public IReadOnlyCollection<Card> Cards => _cards;
    public string? Cards { get; set; }

    // public static Player Create(
    //     Guid id,
    //     Guid roomId,
    //     bool isLeader,
    //     bool readiness,
    //     string playerName,
    //     int score)
    // {
    //     return new Player(
    //         id,
    //         roomId,
    //         isLeader,
    //         readiness,
    //         playerName,
    //         score);
    // }
    public static Player Create(Guid id, string playerName, bool isLeader, Guid roomId)
    {
        return new Player(id, playerName, isLeader, roomId);
    }

    public void SetRoomId(Guid roomId)
    {
        RoomId = roomId;
    }
}