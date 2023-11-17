using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sphere.BL.Game21.Interfaces;
using Sphere.DAL.Game21;
using Sphere.Services.Interfaces;

namespace Sphere.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class Game21Controller : Controller
{
    private readonly IGame21Service _game21Service;
    private readonly IGame _game;
    private readonly IUserStatisticDbApi _userStatisticDbApi;
    private readonly IUserHistoryDbApi _userHistoryDbApi;
    private readonly ILogger<Game21Controller> _logger;

    public Game21Controller(IGame21Service game21Service, IGame game, IUserStatisticDbApi userStatisticDbApi, IUserHistoryDbApi userHistoryDbApi, ILogger<Game21Controller> logger)
    {
        _game21Service = game21Service;
        _game = game;
        _userStatisticDbApi = userStatisticDbApi;
        _userHistoryDbApi = userHistoryDbApi;
        _logger = logger;
    }

    [HttpGet("start_game/{userId:int}")]
    public IActionResult StartGame(int userId) 
    {
        _logger.LogInformation(userId.ToString());
        return Ok(_game21Service.StartGame(userId));
    }
    
    [HttpGet("get_card")]
    public async Task<IActionResult> GetCard() => Ok(await _game21Service.GetCardAsync());

    [HttpGet("pass")]
    public async Task<IActionResult> Pass() => Ok(await _game21Service.PassAsync());

    [HttpGet("get_history/{userId:int}")]
    public async Task<IActionResult> GetHistory(int userId, int historyCount = 5)  // TODO: finish in dbApi
        => Ok(await _game21Service.GetUserHistoryAsync(userId));

    [HttpGet("get_statistic/{userId:int}")]
    public async Task<IActionResult> GetStatistic(int userId)
    {
        var result = await _game21Service.GetUserStatisticAsync(userId);
        _logger.LogInformation(result.Money.ToString());
        return Ok(result);
    }
}