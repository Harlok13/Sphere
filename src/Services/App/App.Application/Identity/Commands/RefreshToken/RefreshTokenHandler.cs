using System.IdentityModel.Tokens.Jwt;
using App.Application.Identity.Commands.Register;
using App.Application.Identity.Repositories;
using App.Application.Identity.Services;
using App.Application.Repositories;
using App.Application.Repositories.UnitOfWork;
using App.Contracts.Identity.Responses;
using App.Domain.Identity.Entities;
using Mediator;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace App.Application.Identity.Commands.RefreshToken;

public class RefreshTokenHandler : ICommandHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    private readonly ILogger<RefreshTokenHandler> _logger;
    private readonly IJwtService _jwtService;
    private readonly IPlayerRepository _playerRepository;
    private readonly IPlayerInfoRepository _playerInfoRepository;
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAppUnitOfWork _appUnitOfWork;
    private readonly IMediator _mediator;

    public RefreshTokenHandler(
        ILogger<RefreshTokenHandler> logger,
        IJwtService jwtService,
        IPlayerRepository playerRepository,
        IPlayerInfoRepository playerInfoRepository,
        IApplicationUserRepository applicationUserRepository,
        IAppUnitOfWork appUnitOfWork,
        UserManager<ApplicationUser> userManager,
        IMediator mediator)
    {
        _logger = logger;
        _jwtService = jwtService;
        _playerRepository = playerRepository;
        _playerInfoRepository = playerInfoRepository;
        _applicationUserRepository = applicationUserRepository;
        _appUnitOfWork = appUnitOfWork;
        _userManager = userManager;
        _mediator = mediator;
    }
    
    public async ValueTask<RefreshTokenResponse> Handle(RefreshTokenCommand command, CancellationToken cT)
    {
        var tokenRequest = command.TokenRequest;

        if (tokenRequest is null)
        {
            throw new Exception();  // TODO: ex
            // return BadRequest("Invalid client request");
        }

        var accessToken = tokenRequest.AccessToken;
        var refreshToken = tokenRequest.RefreshToken;
        var principal = _jwtService.GetPrincipalFromExpiredToken(accessToken);

        if (principal == null)
        {
            // return BadRequest("Invalid access token or refresh token");
            throw new Exception();  // TODO: ex
        }

        var username = principal.Identity.Name;
        var user = await _applicationUserRepository.GetManagedUserByNameAsync(username, cT);

        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            // return BadRequest("Invalid access token or refresh token");
            throw new Exception();  // TODO: ex
        }

        var newAccessToken = _jwtService.CreateToken(principal.Claims.ToArray());
        var newRefreshToken = _jwtService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _applicationUserRepository.UpdateManagedUserAsync(user, cT);
        
        // return new ObjectResult(new
        // {
        //     accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
        //     refreshToken = newRefreshToken
        // });
        return new RefreshTokenResponse(
            new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            newRefreshToken);
    }
}