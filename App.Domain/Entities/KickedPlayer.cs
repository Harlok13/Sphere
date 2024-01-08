using App.Domain.Entities.PlayerEntity;
using App.Domain.Entities.RoomEntity;
using App.Domain.Messages;
using App.Domain.Primitives;

namespace App.Domain.Entities;

public sealed class KickedPlayer : Entity
{
    private KickedPlayer(
        Guid id,
        Guid playerId,
        string playerName,
        Guid whoKickId,
        string whoKickName,
        Guid roomId) : base(id)
    {
        PlayerId = playerId;
        PlayerName = playerName;
        WhoKickId = whoKickId;
        WhoKickName = whoKickName;
        RoomId = roomId;
    }

    public Guid PlayerId { get; private init; }
    public string PlayerName { get; private init; }
    public Guid WhoKickId { get; private init; }
    public string WhoKickName { get; private init; }

    public Guid RoomId { get; private init; }

    public Room Room { get; private init; }

    public static KickedPlayer Create(
        Guid id,
        Guid playerId,
        string playerName,
        Guid whoKickId,
        string whoKickName,
        Guid roomId,
        Room room)
    {
        var kickedPlayer = new KickedPlayer(
            id: id,
            playerId: playerId,
            playerName: playerName,
            whoKickName: whoKickName,
            whoKickId: whoKickId,
            roomId: roomId)
        {
            Room = room
        };

        return kickedPlayer;
    }
}