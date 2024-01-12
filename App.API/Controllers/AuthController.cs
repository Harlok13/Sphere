using App.Application.Identity.Commands.Authenticate;
using App.Application.Identity.Commands.RefreshToken;
using App.Application.Identity.Commands.Register;
using App.Application.Identity.Commands.Revoke;
using App.Application.Identity.Commands.RevokeAll;
using App.Contracts.Identity.Requests;
using App.Contracts.Identity.Responses;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sphere.Controllers;

[Route("auth")]  
[ApiController]
public sealed class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;
    private readonly IMediator _mediator;

    public AuthController(ILogger<AuthController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthenticateResponse>> Authenticate([FromBody] AuthenticateRequest request, CancellationToken cT)
    {
        var command = new AuthenticateCommand(request);
        return Ok(await _mediator.Send(command, cT));
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthenticateResponse>> Register([FromBody] RegisterRequest request, CancellationToken cT)
    {
        var command = new RegisterCommand(request);
        return Ok(await _mediator.Send(command, cT));
    }

    [HttpPost("refresh_token")]
    public async Task<ActionResult<RefreshTokenResponse>> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cT)
    {
        var command = new RefreshTokenCommand(request);
        return Ok(await _mediator.Send(command, cT));
    }

    [Authorize]
    [HttpPost("revoke/{username}")]
    public async Task<IActionResult> Revoke(string username, CancellationToken cT)
    {
        var command = new RevokeCommand(username);
        return Ok(await _mediator.Send(command, cT));
    }

    [Authorize]
    [HttpPost("revoke_all")]
    public async Task<IActionResult> RevokeAll(CancellationToken cT)
    {
        var command = new RevokeAllCommand();
        return Ok(await _mediator.Send(command, cT));
    }
}