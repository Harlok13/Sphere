using App.Application.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/test")]
[ApiController]
public class TestController : Controller
{
    private readonly IAppUnitOfWork _unitOfWork;

    public TestController(IAppUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("[action]/{playerId:guid}")]
    public async Task<IActionResult> Get1(Guid playerId, CancellationToken cT)
    {
        return Ok(await _unitOfWork.RoomRepository.GetByPlayerIdAsync(playerId, cT));
    }
    
    // [HttpGet("{playerId:guid}")]
    // public async Task<IActionResult> Get2(Guid playerId, CancellationToken cT)
    // {
    //     return Ok(await _unitOfWork.RoomRepository.GetByPlayerIdAsNoTrackingAsync2(playerId, cT));
    // }
}