using Sphere.BL.Game21.Interfaces;
using Sphere.DAL.Game21;
using Sphere.DTO.Game21;
using Sphere.Services.Interfaces;

namespace Sphere.Services;

public class Game21Service : IGame21Service
{
    private readonly IGame _game;
    private readonly IUserStatisticDbApi _userStatisticDbApi;
    private readonly IUserHistoryDbApi _userHistoryDbApi;
    private readonly ILogger<Game21Service> _logger;

    public Game21Service(
        IGame game,
        IUserStatisticDbApi userStatisticDbApi,
        IUserHistoryDbApi userHistoryDbApi,
        ILogger<Game21Service> logger
    )
    {
        _game = game;
        _userStatisticDbApi = userStatisticDbApi;
        _userHistoryDbApi = userHistoryDbApi;
        _logger = logger;
    }

    public Game21ResultDto StartGame(int userId) => _game.StartGame(userId);

    public async Task<Game21ResultDto> PassAsync() =>
        await _game.PassAsync(_userStatisticDbApi, _userHistoryDbApi);

    public async Task<Game21ResultDto> GetCardAsync() =>
        await _game.ConAsync(_userStatisticDbApi, _userHistoryDbApi);

    public async Task<UserStatisticDto> GetUserStatisticAsync(int userId) =>
        await _userStatisticDbApi.GetUserStatisticAsync(userId);

    public async Task<UserHistoryModelDto[]> GetUserHistoryAsync(int userId) =>
        await _userHistoryDbApi.GetHistoryAsync(userId);
}