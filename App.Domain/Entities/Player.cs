using App.Domain.Primitives;

namespace App.Domain.Entities;

public class Player : Entity
{
    private readonly List<Card> _cards = new();

    private Player(
        Guid id,
        string playerName,
        Guid roomId,
        int money
    ) : base(id)
    {
        PlayerName = playerName;
        AvatarUrl = "img/avatars/default_avatar.png";  // TODO: hardcode
        RoomId = roomId;
        Money = money;
        IsLeader = default;
        Readiness = default;
        Score = default;
        Cards = default;
        Move = default;
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

    public bool Move { get; private set; }

    public int Money { get; private set; }

    public static Player Create(
        Guid id,
        string playerName,
        Guid roomId,
        int money)
    {
        return new Player(id, playerName, roomId, money);
    }

    public void SetRoomId(Guid roomId)
    {
        RoomId = roomId;
    }

    public void SetIsLeader(bool value)
    {
        IsLeader = value;
    }

    public void ToggleReadiness()
    {
        Readiness = !Readiness;
    }
}