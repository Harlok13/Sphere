using System.Text.Json;
using GameInteraction.Contracts.Data;
using GameInteraction.Domain.Entities;
using GameInteraction.Domain.Entities.PlayerEntity;
using UserInteraction.Contracts.Data;

namespace GameInteraction.Contracts.Mapper;

public class PlayerMapper
{
    // public static PlayerInfoResponse MapPlayerInfoToPlayerInfoResponse(PlayerInfo entity)
    // {
    //     return new PlayerInfoResponse(
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