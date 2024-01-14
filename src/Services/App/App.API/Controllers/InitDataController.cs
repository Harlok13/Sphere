using App.Application.Queries;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

// [Authorize]
[Route("api/init_data")]
[ApiController]
public sealed class InitDataController : Controller
{
    private readonly ILogger<InitDataController> _logger;
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _contextAccessor;
    
    public InitDataController(ILogger<InitDataController> logger, IMediator mediator, IHttpContextAccessor contextAccessor)
    {
        _logger = logger;
        _mediator = mediator;
        _contextAccessor = contextAccessor;
    }

    [HttpGet("{playerId:guid}")]
    public async Task<IActionResult> GetInitData(Guid playerId, CancellationToken cT)
    {
        _logger.LogDebug("{Count}", _contextAccessor?.HttpContext?.User.Claims.Count());
        _logger.LogDebug("{Count}", _contextAccessor?.HttpContext?.User.Identity.Name);
        var query = new GetInitDataQuery(playerId);
        return Ok(await _mediator.Send(query, cT));
    }
}