using App.Application.Queries;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/init_data")]
[ApiController]
public sealed class InitDataController : Controller
{
    private readonly ILogger<InitDataController> _logger;
    private readonly IMediator _mediator;

    public InitDataController(ILogger<InitDataController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet("{playerId:guid}")]
    public async Task<IActionResult> GetInitData(Guid playerId, CancellationToken cT)
    {
        var query = new GetInitDataQuery(playerId);
        return Ok(await _mediator.Send(query, cT));
    }
}