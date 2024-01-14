using System.Text.Json;
using App.Contracts.Data;
using App.Contracts.Identity.Responses;
using App.Contracts.Responses;
using App.Domain.Entities;
using Player = App.Domain.Entities.PlayerEntity.Player;
using PlayerInfo = App.Domain.Entities.PlayerInfoEntity.PlayerInfo;

namespace App.Contracts.Mapper;

public class PlayerMapper
{
    public static PlayerInfoResponse MapPlayerInfoToPlayerInfoResponse(PlayerInfo entity)
    {
        return new PlayerInfoResponse(
            Id: entity.UserId,
            AvatarUrl: entity.AvatarUrl,
            PlayerName: entity.PlayerName,
            Matches: entity.Matches,
            Loses: entity.Loses,
            Wins: entity.Wins,
            Draws: entity.Draws,
            AllExp: entity.AllExp,
            CurrentExp: entity.CurrentExp,
            TargetExp: entity.TargetExp,
            Money: entity.Money,
            Likes: entity.Likes,
            Level: entity.Level,
            Has21: entity.Has21);
    }

    // public static PlayerInfoDto MapPlayerToPlayerInfoDto(PlayerInfo entity)
    // {
    //     return new PlayerInfoDto(
    //         Id: entity.UserId,
    //         AvatarUrl: entity.AvatarUrl,
    //         PlayerName: entity.PlayerName,
    //         Matches: entity.Matches,
    //         Loses: entity.Loses,
    //         Wins: entity.Wins,
    //         Draws: entity.Draws,
    //         AllExp: entity.AllExp,
    //         CurrentExp: entity.CurrentExp,
    //         TargetExp: entity.TargetExp,
    //         Money: entity.Money,
    //         Likes: entity.Likes,
    //         Level: entity.Level,
    //         Has21: entity.Has21);
    // }

    public static PlayerDto MapPlayerToPlayerDto(Player entity)
    {
        
        // TODO: fix
        var cards = entity.Cards is null ? new List<Card>() : JsonSerializer.Deserialize<List<Card>>(entity.Cards);
        return new PlayerDto(
            Id: entity.Id,
            RoomId: entity.RoomId,
            IsLeader: entity.IsLeader,
            Readiness: entity.Readiness,
            PlayerName: entity.PlayerName,
            Score: entity.Score,
            AvatarUrl: entity.AvatarUrl,
            Cards: cards,
            Move: entity.Move,
            Money: entity.Money,
            InGame: entity.InGame,
            Online: entity.Online);
    }

    public static PlayerHistoryResponse MapPlayerHistoryToPlayerHistoryResponse(PlayerHistory entity)
    {
        return new PlayerHistoryResponse(
            Id: entity.Id,
            Score: entity.Score,
            PlayedAt: entity.PlayedAt,
            CardsPlayed: entity.CardsPlayed,
            Result: entity.Result);
    }

    public static IEnumerable<PlayerHistoryResponse> MapManyPlayerHistoryToManyPlayerHistoryResponse(ICollection<PlayerHistory> playerHistories)
    {
        var playerHistoriesResponse = new List<PlayerHistoryResponse>(playerHistories.Count);

        foreach (var playerHistory in playerHistories)
        {
            var playerHistoryResponse = MapPlayerHistoryToPlayerHistoryResponse(playerHistory);
            playerHistoriesResponse.Add(playerHistoryResponse);
        }

        return playerHistoriesResponse;
    }

    public static IEnumerable<PlayerDto> MapManyPlayersToManyPlayersDto(IReadOnlyCollection<Player> players)
    {
        var playersResponse = new List<PlayerDto>(players.Count);

        foreach (var player in players)
        {
            var playerResponse = MapPlayerToPlayerDto(player);
            playersResponse.Add(playerResponse);
        }

        return playersResponse;
    }
}