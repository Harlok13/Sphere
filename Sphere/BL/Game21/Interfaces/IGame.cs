using Sphere.DAL.Game21;
using Sphere.DTO.Game21;

namespace Sphere.BL.Game21.Interfaces;

public interface IGame
{
    public Game21ResultDto StartGame(int userId);
    
    public Task<Game21ResultDto> ConAsync(IUserStatisticDbApi userStatisticDbApi, IUserHistoryDbApi userHistoryDbApi);
    
    public Task<Game21ResultDto> PassAsync(IUserStatisticDbApi userStatisticDbApi, IUserHistoryDbApi userHistoryDbApi);
}