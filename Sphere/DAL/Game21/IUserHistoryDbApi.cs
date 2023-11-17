using Sphere.DTO.Game21;

namespace Sphere.DAL.Game21;

public interface IUserHistoryDbApi
{
    public Task SaveGameHistoryAsync(string score, string cardsPlayed, string gameResult, int userId);

    public Task<UserHistoryModelDto[]> GetHistoryAsync(int userId);
}