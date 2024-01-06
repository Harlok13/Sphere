using System.Collections.Immutable;
using System.Text;
using App.Domain.DomainEvents.RoomDomainEvents;
using App.Domain.DomainResults;
using App.Domain.DomainResults.CustomResults;
using App.Domain.Entities.PlayerEntity;
using App.Domain.Enums;
using App.Domain.Extensions;
using App.Domain.Messages;

namespace App.Domain.Entities.RoomEntity;

public sealed partial class Room
{
    public DomainResult CanStartGame(Guid leaderId)
    {
        lock (_lock)
        {
            var roomLeader = _players.SingleOrDefault(p => p.Id == leaderId);
            if (roomLeader is null)
                return new DomainError(
                    Message.Error.Room.PlayerNotFound(nameof(StartGame), leaderId));

            if (_players.Count < 2)
                return new DomainFailure(
                    Message.Failure.Room.CanStartGame.NotEnoughPlayers());

            if (!roomLeader.IsLeader)
                return new DomainFailure(
                    Message.Failure.Room.CanStartGame.NotLeader());

            if (!roomLeader.Readiness)
                return new DomainFailure(
                    Message.Failure.Room.CanStartGame.NotReadiness());

            if (!_players.All(p => p.Readiness))
            {
                /* player name can't be null */
                var players = _players
                    .Where(p => !p.Readiness)
                    .Select(p => new { p.PlayerName, p.Id })
                    .ToImmutableArray();

                var playerNames = new StringBuilder();
                playerNames.AppendJoin(", ", players.Select(x => x.PlayerName));

                return new DomainNotificationFailure(
                    Reason: Message.Failure.Room.CanStartGame.PlayersNotReady(playerNames.ToString()),
                    PlayerIds: players.Select(p => p.Id),
                    NotificationForPlayers: Message.Notification.Room.CanStartGame.NotReady());
            }

            if (!_players.All(p => p.Money >= StartBid))
            {
                var players = _players
                    .Where(p => p.Money < StartBid)
                    .Select(p => new { p.PlayerName, p.Id })
                    .ToImmutableArray();

                var playerNames = new StringBuilder();
                playerNames.AppendJoin(", ", players.Select(x => x.PlayerName));

                return new DomainNotificationFailure(
                    Reason: Message.Failure.Room.CanStartGame.SomeoneNotEnoughMoney(playerNames.ToString()),
                    PlayerIds: players.Select(p => p.Id),
                    NotificationForPlayers: Message.Notification.Room.CanStartGame.NotEnoughMoney());
            }

            return new DomainSuccessResult();
        }
    }

    /// <summary>
    /// Retrieves the next card from the cards deck.
    /// </summary>
    /// <param name="playerId">The ID of the player who is getting the next card.</param>
    /// <returns>
    /// A <see cref="DomainResult"/> object containing the next card if successful,
    /// or one of the following:
    /// <list type="bullet">
    ///     <item><see cref="DomainError"/>: If there is an error retrieving the card deck.</item>
    ///     <item><see cref="DomainFailure"/>: If the deck is out of cards.</item>
    /// </list>
    /// </returns>
    private DomainResult GetNextCard(Guid playerId)
    {
        var cardsDeckResult = GetCardsDeck();
        if (cardsDeckResult is DomainError cardsDeckError)
            return cardsDeckError;

        if (!cardsDeckResult.TryFromDomainResult(out List<CardInDeck>? cardsDeck, out DomainError? error))
            return error!;

        var cardInDeck = cardsDeck!.FirstOrDefault();
        if (cardInDeck is null)
            return new DomainError(
                Message.Failure.Room.GetNextCard.DeckIsOut());

        cardsDeck!.Remove(cardInDeck);

        var card = Card.Create(Guid.NewGuid(), playerId: playerId, cardInDeck: cardInDeck);

        return new DomainSuccessResult<Card>(card);
    }

    public DomainResult StartGame(Guid leaderId, IEnumerable<CardInDeck> cardsDeck)
    {
        UpdateCardsDeck(cardsDeck); // TODO: ref

        lock (_players)
        {
            foreach (var indexAkaDelay in Enumerable.Range(0, _players.Count))
            {
                var player = _players[indexAkaDelay];

                var cardResult = GetNextCard(player.Id);
                if (!cardResult.Success) return cardResult;
                // if (cardResult is DomainError cardError) return cardError;
                // if (cardResult is DomainFailure cardFailure) return cardFailure;
                if (!cardResult.TryFromDomainResult(out Card? card, out DomainError? error)) return error!;

                var delayMs = indexAkaDelay * 1000;

                var playerStartGameResult = player.StartGame(startBid: StartBid, card: card!, delayMs: delayMs);
                if (playerStartGameResult is DomainFailure playerStartGameFailure)
                    return playerStartGameFailure;

                if (playerStartGameResult is DomainError playerStartGameError)
                    return playerStartGameError;

                IncreaseBank(StartBid);

                if (player.Id == leaderId)
                {
                    player.SetMove(true);
                }
            }

            UpdateCardsDeck(_cardsDeck);
            return new DomainSuccessResult();
        }
    }

    public DomainResult TransferLeadership(Guid senderId, Guid receiverId)
    {
        lock (_lock)
        {
            var sender = Players.SingleOrDefault(p => p.Id == senderId);
            if (sender is null)
                return new DomainError(
                    Message.Error.Room.PlayerNotFound(nameof(TransferLeadership), senderId));

            var receiver = Players.SingleOrDefault(p => p.Id == receiverId);
            if (receiver is null)
                return new DomainError(
                    Message.Error.Room.PlayerNotFound(nameof(TransferLeadership), receiverId));

            if (!sender.IsLeader)
                return new DomainFailure(
                    Message.Failure.Room.TransferLeadership.NotLeader(receiver.PlayerName));

            sender.SetIsLeader(false);
            receiver.SetIsLeader(true);

            _domainEvents.Add(new TransferredLeadershipDomainEvent(
                SenderId: senderId,
                ReceiverId: receiverId,
                NotificationForSender:
                Message.Notification.Room.TransferLeadership.SuccessTransfer(receiver.PlayerName),
                NotificationForReceiver: Message.Notification.Room.TransferLeadership.ReceiveLeadership(
                    sender.PlayerName)));

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

            return new DomainSuccessResult<Player>(reconnectPlayer);
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

            if (player.InGame)
                return new DomainFailure(
                    Message.Failure.Room.RemovePlayerFromRoom.InGame());

            RemovePlayer(player);

            if (Players.Count > 0)
            {
                var setNewRoomLeaderResult = SetNewRoomLeader();
                if (setNewRoomLeaderResult is DomainError setNewRoomLeaderError)
                    return new DomainError(setNewRoomLeaderError.Reason);
            }

            return new DomainSuccessResult<int>(player.Money);
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
        lock (_kickedPlayers)
        {
            if (_players.Count == RoomSize)
                return new DomainFailure(
                    Message.Failure.Room.CanPlayerJoin.RoomIsFull(RoomName));

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

        lock (_players)
        {
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
    }

    public DomainResult KickPlayer(Guid initiatorId, Guid kickedId)
    {
        lock (_players)
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
    }

    public DomainResult PlayerHit(Guid playerId)
    {
        lock (_players)
        {
            var player = _players.SingleOrDefault(p => p.Id == playerId);
            if (player is null)
                return new DomainError(
                    Message.Error.Room.PlayerNotFound(nameof(PlayerHit), playerId));

            var getCardsCountResult = player.GetCardsCount();
            if (!getCardsCountResult.TryFromDomainResult(out int cardsCount, out DomainError? getCardsCountError))
                return getCardsCountError!;

            if (cardsCount >= MaxCardsCount)
                return new DomainFailure(
                    Message.Failure.Room.PlayerHit.MaxCards(MaxCardsCount));

            var cardResult = GetNextCard(playerId);
            if (!cardResult.Success) return cardResult;
            // if (cardResult is DomainError cardError) return cardError;
            // if (cardResult is DomainFailure cardFailure) return cardFailure;
            if (!cardResult.TryFromDomainResult(out Card? card, out DomainError? error)) return error!;

            var hitResult = player.Hit(card!);
            if (hitResult is DomainError hitError) return hitError;

            return new DomainSuccessResult();
        }
    }

    public DomainResult PlayerStay(Guid playerId)
    {
        lock (_players)
        {
            var player = _players.SingleOrDefault(p => p.Id == playerId);
            if (player is null)
                return new DomainError(
                    Message.Error.Room.PlayerNotFound(nameof(PlayerHit), playerId));

            player.Stay();

            return new DomainSuccessResult();
        }
    }

    public DomainResult PlayerToggleReadiness(Guid playerId)
    {
        lock (_players)
        {
            var player = _players.SingleOrDefault(p => p.Id == playerId);
            if (player is null)
                return new DomainError(
                    Message.Error.Room.PlayerNotFound(nameof(PlayerToggleReadiness), playerId));

            player.ToggleReadiness();

            return new DomainSuccessResult();
        }
    }

    public bool PassTurn()
    {
        lock (_players)
        {
            if (_players.Any(p => p.MoveStatus == MoveStatus.None))
            {
                _players
                    .First(p => p.MoveStatus == MoveStatus.None)
                    .SetMove(true);

                return true;
            }

            return false;
        }
    }
}