using System.Collections.Immutable;
using App.Domain.DomainEvents.RoomDomainEvents;
using App.Domain.DomainResults;
using App.Domain.DomainResults.CustomResults;
using App.Domain.Entities.PlayerEntity;
using App.Domain.Enums;
using App.Domain.Messages;

namespace App.Domain.Entities.RoomEntity;

public sealed partial class Room
{
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

    public DomainResult TransferLeadership(Guid senderId, Guid receiverId)
    {
        lock (_lock)
        {
            var sender = Players.SingleOrDefault(p => p.Id == senderId);
            if (sender is null)
                return new DomainError(
                    Message.Error.Room.PlayerNotFound(nameof(Message.Error.Room.TransferLeadership), senderId));

            var receiver = Players.SingleOrDefault(p => p.Id == receiverId);
            if (receiver is null)
                return new DomainError(
                    Message.Error.Room.PlayerNotFound(nameof(Message.Error.Room.TransferLeadership), receiverId));

            if (!sender.IsLeader)
                return new DomainFailure(
                    Message.Failure.Room.TransferLeadership.NotLeader(receiver.PlayerName));

            sender.SetIsLeader(false);
            receiver.SetIsLeader(true);

            _domainEvents.Add(new TransferredLeadershipDomainEvent(
                SenderId: senderId,
                ReceiverId: receiverId,
                NotificationForSender: Message.Notification.Room.TransferLeadership.SuccessTransfer(receiver.PlayerName),
                NotificationForReceiver: Message.Notification.Room.TransferLeadership.ReceiveLeadership(sender.PlayerName)));

            return new DomainSuccessResult();
        }
    }


    public DomainResult ReconnectPlayer(Guid playerId, string newConnectionId)
    {
        lock (_lock)
        {
            var reconnectPlayer = Players.SingleOrDefault(p => p.Id == playerId);
            if (reconnectPlayer is null)
                return new DomainError(
                    Message.Error.Room.PlayerNotFound(nameof(ReconnectPlayer), playerId));

            reconnectPlayer.SetOnline(true, newConnectionId);

            return new ReconnectPlayerDomainResult(reconnectPlayer);
        }
    }
    
    public DomainResult DisconnectPlayer(Guid disconnectPlayerId)
    {
        lock (_lock)
        {
            var disconnectPlayer = Players.SingleOrDefault(p => p.Id == disconnectPlayerId);
            if (disconnectPlayer is null)
                return new DomainError(
                    Message.Error.Room.PlayerNotFound(nameof(DisconnectPlayer), disconnectPlayerId));

            disconnectPlayer.SetOnline(false);

            var playersOnline = Players
                .Where(p => p.Online)
                .ToImmutableArray();

            if (playersOnline.Length < 1)
            {
                /* this action will delete the room */
                SetPlayersInRoom(0);
            }

            return new DomainSuccessResult();
        }
    }
    
    public DomainResult RemovePlayerFromRoom(Guid playerId)
    {
        lock (_lock)
        {
            var player = _players.SingleOrDefault(p => p.Id == playerId);
            if (player is null)
                return new DomainError(
                    Message.Error.Room.PlayerNotFound(nameof(RemovePlayerFromRoom), playerId));

            RemovePlayer(player);

            if (Players.Count > 0)
            {
                var setNewRoomLeaderResult = SetNewRoomLeader();
                if (setNewRoomLeaderResult is DomainError setNewRoomLeaderError)
                    return new DomainError(setNewRoomLeaderError.Reason);
            }


            return new RemovePlayerFromRoomDomainResult(player.Money);
        }
    }

    private DomainResult SetNewRoomLeader(Guid? playerId = null)
    {
        lock (_lock)
        {
            var newLeader = playerId is null
                ? _players.FirstOrDefault()
                : _players.SingleOrDefault(p => p.Id == playerId);

            if (newLeader is null)
                return new DomainError(
                    Message.Error.Room.PlayerNotFound(nameof(SetNewRoomLeader), playerId));

            newLeader.SetIsLeader(value: true);
            SetRoomName(name: newLeader.PlayerName, withPostfix: true);
            SetAvatarUrl(newLeader.AvatarUrl);

            return new DomainSuccessResult();
        }
    }

    public DomainResult CanPlayerJoin(Guid playerId)
    {
        if (Players.Count == RoomSize)
            return new DomainFailure(
                Message.Failure.Room.CanPlayerJoin.RoomIsFull(RoomName));

        lock (_kickedPlayers)
        {
            var wasKicked = _kickedPlayers.Any(p => p.PlayerId == playerId);
            if (wasKicked)
            {
                /* the table cannot contain a null value for the name, so no check is needed */
                var whoKickName = _kickedPlayers
                    .Where(p => p.PlayerId == playerId)
                    .Select(p => p.WhoKickName)
                    .Single();

                return new DomainFailure(Message.Failure.Room.CanPlayerJoin.PlayerWasKicked(
                    roomName: RoomName,
                    whoKickName: whoKickName));
            }
        }

        return new DomainSuccessResult();
    }

    public DomainResult JoinToRoom(Guid playerId, string playerName, int money, string connectionId)
    {
        if (money < LowerStartMoneyBound)
            return new DomainFailure(
                Message.Failure.Room.JoinToRoom.NotEnoughMoney(RoomName));

        var isLeader = _players.Count < 1;
        var player = Player.Create(
            id: playerId,
            playerName: playerName,
            roomId: Id,
            money: money,
            connectionId: connectionId,
            isLeader: isLeader,
            room: this);

        AddNewPlayer(player);

        return new DomainSuccessResult();
    }

    public DomainResult KickPlayer(Guid initiatorId, Guid kickedId)
    {
        var initiator = _players.SingleOrDefault(p => p.Id == initiatorId);
        if (initiator is null)
            return new DomainError(
                Message.Error.Room.PlayerNotFound(nameof(KickPlayer), initiatorId));


        var kickedPlayer = _players.SingleOrDefault(p => p.Id == kickedId);
        if (kickedPlayer is null)
            return new DomainError(
                Message.Error.Room.PlayerNotFound(nameof(KickPlayer), kickedId));


        if (!initiator.IsLeader)
            return new DomainFailure(
                Message.Failure.Room.KickPlayer.NotLeader());

        if (kickedPlayer.InGame)
            return new DomainFailure(
                Message.Failure.Room.KickPlayer.PlayerInGame(kickedPlayer.PlayerName));

        AddKickedPlayer(initiator: initiator, kickedPlayer: kickedPlayer);

        return new DomainSuccessResult();
    }

    public DomainResult PlayerHit(Guid playerId, Card card)
    {
        var player = _players.SingleOrDefault(p => p.Id == playerId);
        if (player is null)
            return new DomainError(
                Message.Error.Room.PlayerNotFound(nameof(PlayerHit), playerId));

        player.Hit(card);

        return new DomainSuccessResult();
    }

    public DomainResult PlayerStay(Guid playerId)
    {
        var player = _players.SingleOrDefault(p => p.Id == playerId);
        if (player is null)
            return new DomainError(
                Message.Error.Room.PlayerNotFound(nameof(PlayerHit), playerId));

        player.Stay();

        return new DomainSuccessResult();
    }
}