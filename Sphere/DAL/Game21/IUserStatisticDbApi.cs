using Sphere.DTO.Game21;

namespace Sphere.DAL.Game21;

public interface IUserStatisticDbApi
{
    public Task CreateUserStatisticAsync(int userId);
    public Task<UserStatisticDto> GetUserStatisticAsync(int userId);

    public Task UserWinActionAsync(int userId);
    public Task UserLoseActionAsync(int userId);
    public Task UserDrawActionAsync(int userId);
    public Task UserHas21ActionAsync(int userId);
}