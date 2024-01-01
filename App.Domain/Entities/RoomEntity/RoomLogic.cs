using System.Collections.Immutable;
using App.Domain.DomainEvents.RoomDomainEvents;
using App.Domain.Entities.PlayerEntity;
using App.Domain.Enums;
using App.Domain.Messages;
using App.Domain.Shared;
using App.Domain.Shared.ResultImplementations;

namespace App.Domain.Entities.RoomEntity;

public sealed partial class Room
{
    public record DomainResult(bool Success);

    public record DomainFailure(string Reason) : DomainResult(false);

    public record DomainError(string Reason) : DomainResult(false);

    public record DomainSuccessResult(string NotificationText) : DomainResult(true);
    
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

    public sealed record TransferLeadershipDto(string ReceiverName, string SenderName);

    public record TransferLeadershipResult(bool Success);

    public record TransferLeadershipFailure(string Reason) : TransferLeadershipResult(false);

    public record TransferLeadershipError(string Reason) : TransferLeadershipResult(false);

    public sealed record PlayerNotFound(string Reason) : TransferLeadershipError(false);

    public Result<TransferLeadershipDto> TransferLeadership(Guid senderId, Guid receiverId)
    {
        lock (_lock)
        {
            List<Error> errors = new();
            
            var sender = Players.SingleOrDefault(p => p.Id == senderId);
            if (sender is null)
            {
                errors.Add(new Error(ErrorMessages.Room.TransferLeadership.SenderNotFound()));
            }
        
            var receiver = Players.SingleOrDefault(p => p.Id == receiverId);
            if (receiver is null)
            {
                errors.Add(new Error(ErrorMessages.Room.TransferLeadership.ReceiverNotFound()));
            }

            if (errors.Count > 0) return NotFoundResult<TransferLeadershipDto>.Create(errors);
            
            if (!sender!.IsLeader)

            sender!.SetIsLeader(false);
            receiver!.SetIsLeader(true);
            
            _domainEvents.Add(new TransferredLeadershipDomainEvent(
                SenderId: senderId,
                ReceiverId: receiverId,
                NotificationForSender: ,
                NotificationForReceiver: ));

            return SuccessResult<TransferLeadershipDto>.Create(new TransferLeadershipDto(
                ReceiverName: receiver.PlayerName,
                SenderName: sender.PlayerName));
        }
    }


    public Result<Player> ReconnectPlayer(Guid playerId, string newConnectionId)
    {
        lock (_lock)
        {
            var reconnectPlayer = Players.SingleOrDefault(p => p.Id == playerId);
            if (reconnectPlayer is null)
            {
                return NotFoundResult<Player>.Create(
                    new Error(ErrorMessages.Room.ReconnectPlayer.PlayerNotFound(playerId)));
            }
            
            reconnectPlayer.SetOnline(true, newConnectionId);
            
            return SuccessResult<Player>.Create(reconnectPlayer);
        }
    }

    // public sealed record DisconnectPlayerDto(bool NeedRemoveRoom);
    
    public Result DisconnectPlayer(Guid disconnectPlayerId)
    {
        lock (_lock)
        {
            var disconnectPlayer = Players.SingleOrDefault(p => p.Id == disconnectPlayerId);
            if (disconnectPlayer is null)
            {
                return Result.Create(
                    isSuccess: false,
                    error: new Error(ErrorMessages.Room.DisconnectPlayer.PlayerNotFound(disconnectPlayerId)));
            }
            
            disconnectPlayer.SetOnline(false);

            var playersOnline = Players
                .Where(p => p.Online)
                .ToImmutableArray();

            // var needRemoveRoom = false;
            if (playersOnline.Length < 1)
            {
             /* this action will delete the room */   
                SetPlayersInRoom(0);
                // needRemoveRoom = true;
                // _domainEvents.Add(new RemovedRoomDomainEvent(Id));
            }

            // return SuccessResult<DisconnectPlayerDto>.Create(new DisconnectPlayerDto(needRemoveRoom));
            return Result.CreateSuccess();
        }
    }
    
    public sealed record RemovePlayerFromRoomDto(int IncrementMoney);
    
    public Result<RemovePlayerFromRoomDto> RemovePlayerFromRoom(Guid playerId)
    {
        lock (_lock)
        {
            var player = _players.SingleOrDefault(p => p.Id == playerId);
            if (player is null)
            {
                return InvalidResult<RemovePlayerFromRoomDto>.Create(
                    new Error(ErrorMessages.Room.RemoveFromRoom.PlayerNotFound(playerId)));
            }
            
            // _players.Remove(player);
            // _domainEvents.Add(new RemovedPlayerDomainEvent(
            //     RoomId: Id,
            //     PlayerId: playerId,
            //     ConnectionId: player.ConnectionId,
            //     PlayersInRoom: PlayersInRoom));
            RemovePlayer(player);
            
            // SetPlayersInRoom(Players.Count);

            // var needRemoveRoom = false;

            if (Players.Count > 0)
            {
                var setNewRoomLeaderResult = SetNewRoomLeader();
                if (setNewRoomLeaderResult.IsFailure)
                {
                    return InvalidResult<RemovePlayerFromRoomDto>
                        .Create(setNewRoomLeaderResult.Errors!);
                }
            }
            // else
            // {
            //     needRemoveRoom = true;
            //     _domainEvents.Add(new RemovedRoomDomainEvent(Id));
            // }

            return SuccessResult<RemovePlayerFromRoomDto>.Create(new RemovePlayerFromRoomDto(
                IncrementMoney: player.Money
                // NeedRemoveRoom: needRemoveRoom
            ));
        }
    }
    
    private Result SetNewRoomLeader(Guid? playerId = null)
    {
        lock (_lock)
        {
            var newLeader = playerId is null
                ? _players.FirstOrDefault()
                : _players.SingleOrDefault(p => p.Id == playerId);

            if (newLeader is null)
            {
                return Result.Create(
                    isSuccess: false,
                    error: new Error(ErrorMessages.Room.SetNewRoomLeader.PlayerNotFound()));
            }

            newLeader.SetIsLeader(value: true);
            SetRoomName(name: newLeader.PlayerName, withPostfix: true);
            SetAvatarUrl(newLeader.AvatarUrl);

            return Result.CreateSuccess();
        }
    }

    public record CanPlayerJoinResult(bool Success);

    public record CanPlayerJoinFailure(string Reason) : CanPlayerJoinResult(false);
    
    public sealed record RoomIsFull(string Reason) : CanPlayerJoinFailure(Reason);

    public sealed record PlayerWasKicked(string Reason) : CanPlayerJoinFailure(Reason);

    public CanPlayerJoinResult CanPlayerJoin(Guid playerId)
    {
        if (Players.Count == RoomSize)
        {
            return new RoomIsFull(ErrorMessages.Room.CanPlayerJoin.RoomIsFull(RoomName));
        }

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
                
                return new PlayerWasKicked(ErrorMessages.Room.CanPlayerJoin.PlayerWasKicked(
                    roomName: RoomName,
                    whoKickName: whoKickName));
            }
        }

        return new CanPlayerJoinResult(true);
    }
    
    public Result JoinToRoom(Guid playerId, string playerName, int money, string connectionId)
    {
        if (money < LowerStartMoneyBound)
        {
            return Result.Create(
                isSuccess: false,
                error: new Error(""));
        }
        
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

        return Result.CreateSuccess();
    }

    public record KickPlayerResult(bool Success);

    public record KickPlayerError(string Reason) : KickPlayerResult(false);

    public record KickPlayerFailure(string Reason) : KickPlayerResult(false);

    public sealed record NotLeader(string Reason) : KickPlayerFailure(Reason);

    public sealed record PlayerInGame(string Reason) : KickPlayerFailure(Reason);

    public sealed record PlayerNotFound(string Reason) : KickPlayerError(Reason);
    
    public KickPlayerResult KickPlayer(Guid initiatorId, Guid kickedId)
    {
        var initiator = _players.SingleOrDefault(p => p.Id == initiatorId);
        if (initiator is null) return new PlayerNotFound("initiator with id not found.");
        

        var kickedPlayer = _players.SingleOrDefault(p => p.Id == kickedId);
        if (kickedPlayer is null) return new PlayerNotFound("kick....");
        

        if (!initiator.IsLeader) return new NotLeader(
            NotificationMessages.Room.KickPlayer.NotLeader());
        
        if (kickedPlayer.InGame) return new PlayerInGame(
            NotificationMessages.Room.KickPlayer.PlayerInGame(kickedPlayer.PlayerName));

        AddKickedPlayer(initiator: initiator, kickedPlayer: kickedPlayer);

        return new KickPlayerResult(true);
    }
}