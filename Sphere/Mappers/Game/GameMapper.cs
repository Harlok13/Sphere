using Sphere.DAL.Models.Redis;
using Sphere.DTO.Game21;

namespace Sphere.Mappers.Game;

public static class GameMapper
{
    public static PlayerModel MapRequestPlayerDtoToPlayerModel(RequestPlayerDto model)
    {
        return new PlayerModel
        {
            PlayerId = model.PlayerId,
            IsLeader = model.IsLeader,
            readiness = model.readiness,
            PlayerName = model.PlayerName
        };
    }

    public static ResponsePlayerDto MapPlayerModelToResponsePlayerDto(PlayerModel model)
    {
        return new ResponsePlayerDto
        {
            PlayerId = model.PlayerId,
            IsLeader = model.IsLeader,
            readiness = model.readiness,
            PlayerName = model.PlayerName
        };
    }
}