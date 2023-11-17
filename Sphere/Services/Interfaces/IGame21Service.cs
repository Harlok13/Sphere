using Sphere.DTO.Game21;

namespace Sphere.Services.Interfaces;

public interface IGame21Service
{
    public Game21ResultDto StartGame(int userId);
    
    public Task<Game21ResultDto> GetCardAsync();
    
    public Task<Game21ResultDto> PassAsync();

    public Task<UserStatisticDto> GetUserStatisticAsync(int userId);

    public Task<UserHistoryModelDto[]> GetUserHistoryAsync(int userId);
}