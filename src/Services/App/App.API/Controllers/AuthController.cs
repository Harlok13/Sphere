using App.Contracts.Identity.Requests;
using App.Contracts.Identity.Responses;
using App.Domain.Entities.PlayerInfoEntity;
using App.GrpcClient.IdentityClient;
using App.GrpcClient.UserInteractionClient;
using AutoMapper;
using Duende.IdentityServer.Models;
using GrpcIdentityClient;
using GrpcUserInteractionClient;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/auth")]
[ApiController]
public sealed class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;
    private readonly IMediator _mediator;
    private readonly IIdentityClient _identityClient;
    private readonly IUserInteractionClient _userInteractionClient;
    private readonly IMapper _mapper;

    public AuthController(
        ILogger<AuthController> logger,
        IMediator mediator,
        IIdentityClient identityClient, 
        IUserInteractionClient userInteractionClient,
        IMapper mapper)
    {
        _logger = logger;
        _mediator = mediator;
        _identityClient = identityClient;
        _userInteractionClient = userInteractionClient;
        _mapper = mapper;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthenticateResponse>> Authenticate([FromBody] AuthenticateRequest request, CancellationToken cT)
    {
        var grpcRequest = _mapper.Map<GrpcAuthenticateRequest>(request);
        return Ok(await _identityClient.AuthenticateAsync(grpcRequest, cT));
        // var command = new AuthenticateCommand(request);
        // return Ok(await _mediator.Send(command, cT));
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthenticateResponse>> Register([FromBody] RegisterRequest request, CancellationToken cT)
    {
        GrpcRegisterRequest grpcRegisterRequest = _mapper.Map<GrpcRegisterRequest>(request);
        GrpcRegisterResponse registerResponse = await _identityClient.RegisterAsync(grpcRegisterRequest, cT);
        
        await _userInteractionClient.CreatePlayerInfoAsync(new GrpcCreatePlayerInfoRequest
            { PlayerName = request.UserName, UserId = registerResponse.UserId }, cT);

        GrpcAuthenticateResponse grpcAuthenticateResponse = await _identityClient.AuthenticateAsync(new GrpcAuthenticateRequest 
            { Email = registerResponse.Email, Password = registerResponse.Password}, cT);

        GrpcGetPlayerInfoResponse grpcPlayerInfo = await _userInteractionClient.GetPlayerInfoAsync(new GrpcGetPlayerInfoRequest
            { PlayerId = registerResponse.UserId}, cT);

        PlayerInfoResponse playerInfoResponse = _mapper.Map<PlayerInfoResponse>(grpcPlayerInfo);

        var response = new AuthenticateResponse(
            PlayerId: Guid.Parse(grpcAuthenticateResponse.PlayerId),
            PlayerName: grpcAuthenticateResponse.PlayerName,
            Email: grpcAuthenticateResponse.Email,
            Token: grpcAuthenticateResponse.Token,
            RefreshToken: grpcAuthenticateResponse.RefreshToken,
            PlayerInfo: playerInfoResponse);
        // AuthenticateResponse response = _mapper.Map<AuthenticateResponse>(grpcAuthenticateResponse);
        // _mapper.Map(playerInfoResponse, response);

        return Ok(response);

        // return Ok(await _identityClient.RegisterAsync(request));
        // var command = new RegisterCommand(request);
        // return Ok(await _mediator.Send(command, cT));
    }

    [HttpPost("refresh_token")]
    public async Task<ActionResult<RefreshTokenResponse>> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cT)
    {
        // var command = new RefreshTokenCommand(request);
        // return Ok(await _mediator.Send(command, cT));
        var grpcRequest = _mapper.Map<GrpcRefreshTokenRequest>(request);
        return Ok(await _identityClient.RefreshTokenAsync(grpcRequest, cT));
    }

    // [Authorize]
    // [HttpPost("revoke/{username}")]
    // public async Task<IActionResult> Revoke(string username, CancellationToken cT)
    // {
    //     var command = new RevokeCommand(username);
    //     return Ok(await _mediator.Send(command, cT));
    // }

    [Authorize]
    [HttpPost("revoke/{username}")] // TODO: use body
    public async Task<IActionResult> Revoke(string username, CancellationToken cT)
    {
        // var command = new RevokeCommand(username);
        // return Ok(await _mediator.Send(command, cT));
        return Ok(await _identityClient.RevokeAsync(new GrpcRevokeRequest { UserName = username }, cT));
    }

    [Authorize]
    [HttpPost("revoke_all")]
    public async Task<IActionResult> RevokeAll(CancellationToken cT)
    {
        // var command = new RevokeAllCommand();
        // return Ok(await _mediator.Send(command, cT));
        return Ok(await _identityClient.RevokeAllAsync(new GrpcRevokeAllRequest(), cT));
    }
}