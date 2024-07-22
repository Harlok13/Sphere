using UserInteraction.Contracts.Data;
using UserInteraction.Domain.Entities;
using UserInteraction.Domain.Entities.PlayerInfoEntity;

namespace UserInteraction.Contracts.Mapper;

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

    public static PlayerInfoDto MapPlayerInfoToPlayerInfoDto(PlayerInfo entity)
    {
        return new PlayerInfoDto(
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

    public static IEnumerable<PlayerHistoryDto> MapManyPlayerHistoryToManyPlayerHistoryDtos(
        ICollection<PlayerHistory> playerHistories)
    {
        var playerHistoryDtos = new List<PlayerHistoryDto>(playerHistories.Count);

        foreach (var playerHistory in playerHistories)
        {
            var playerHistoryResponse = MapPlayerHistoryToPlayerHistoryResponse(playerHistory);
            playerHistoryDtos.Add(playerHistoryResponse);
        }

        return playerHistoryDtos;
    }
    
    private static PlayerHistoryDto MapPlayerHistoryToPlayerHistoryResponse(PlayerHistory entity)
    {
        return new PlayerHistoryDto(
            Id: entity.Id,
            Score: entity.Score,
            PlayedAt: entity.PlayedAt,
            CardsPlayed: entity.CardsPlayed,
            Result: entity.Result);
    }

    public static PlayerInfo MapPlayerInfoDtoToPlayerInfo(PlayerInfoDto friendDto)
    {
        throw new NotImplementedException();
    }
}