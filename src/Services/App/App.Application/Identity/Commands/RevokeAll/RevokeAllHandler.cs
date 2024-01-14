using App.Application.Identity.Commands.Revoke;
using App.Application.Identity.Repositories;
using App.Application.Identity.Services;
using App.Application.Repositories;
using App.Application.Repositories.UnitOfWork;
using App.Domain.Identity.Entities;
using Mediator;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace App.Application.Identity.Commands.RevokeAll;

public class RevokeAllHandler : ICommandHandler<RevokeAllCommand, bool>
{
    private readonly ILogger<RevokeAllHandler> _logger;
    private readonly IJwtService _jwtService;
    private readonly IPlayerRepository _playerRepository;
    private readonly IPlayerInfoRepository _playerInfoRepository;
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAppUnitOfWork _appUnitOfWork;
    private readonly IMediator _mediator;

    public RevokeAllHandler(
        ILogger<RevokeAllHandler> logger,
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
    
    public async ValueTask<bool> Handle(RevokeAllCommand command, CancellationToken cT)
    {
        var users = await _applicationUserRepository.GetAllManagedUsersAsync(cT);
        foreach (var user in users)
        {
            user.RefreshToken = null;
            await _applicationUserRepository.UpdateManagedUserAsync(user, cT);
        }

        return true;
    }
}