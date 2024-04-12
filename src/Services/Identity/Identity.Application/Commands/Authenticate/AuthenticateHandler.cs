// using Identity.Application.Repositories;
// using Identity.Application.Repositories.UnitOfWork;
// using Identity.Application.Services;
// using Identity.Contracts.Requests;
// using Identity.Contracts.Responses;
// using Mediator;
// using Microsoft.Extensions.Logging;
//
// namespace Identity.Application.Commands.Authenticate;
//
// public class AuthenticateHandler : ICommandHandler<AuthenticateCommand, AuthenticateResponse>
// {
//     private readonly ILogger<AuthenticateHandler> _logger;
//     private readonly IJwtService _jwtService;
//     private readonly IIdentityUnitOfWork _unitOfWork;
//
//     public AuthenticateHandler(
//         ILogger<AuthenticateHandler> logger,
//         IJwtService jwtService,
//         IIdentityUnitOfWork unitOfWork)
//     {
//         _logger = logger;
//         _jwtService = jwtService;
//         _unitOfWork = unitOfWork;
//     }
//
//     public async ValueTask<AuthenticateResponse> Handle(AuthenticateCommand command, CancellationToken cT)
//     {
//         command.Deconstruct(out AuthenticateRequest request);
//         
//         _logger.LogDebug($"Auth request\nEmail: {request.Email}\nPassword: {request.Password}");
//
//         var managedUser = await _unitOfWork.ApplicationUserRepository.GetManagedUserByEmailAsync(request.Email, cT);
//
//         if (managedUser == null)
//         {
//             // return BadRequest("Bad credentials");
//             throw new Exception();  // TODO: ex
//         }
//
//         var isPasswordValid = await _unitOfWork.ApplicationUserRepository.CheckManagedUserPasswordAsync(managedUser, request.Password, cT);
//
//         if (!isPasswordValid)
//         {
//             // return BadRequest("Bad credentials");
//             _logger.LogDebug(isPasswordValid.ToString(), "password");
//             throw new Exception();  // TODO: ex
//         }
//
//         var user = await _unitOfWork.ApplicationUserRepository.GetUserByEmailAsync(request.Email, cT);
//
//         if (user is null)
//         {
//             // return Unathorized();
//             throw new Exception();  // TODO: ex
//         }
//
//         var roleIds = await _unitOfWork.ApplicationUserRepository.GetRoleIdsAsync(user, cT);
//         var roles = await _unitOfWork.ApplicationUserRepository.GetRoleNamesAsync(roleIds, cT);
//
//         var accessToken = _jwtService.GetJwtToken(user, roles);
//         user.RefreshToken = _jwtService.GenerateRefreshToken();
//         user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtService.RefreshTokenValidityInDays);
//
//         await _unitOfWork.SaveChangesAsync(cT);
//         var playerInfoResult = await _unitOfWork.PlayerInfoRepository.GetPlayerInfoByIdAsync(user.Id, cT);
//         if (!playerInfoResult.TryFromResult(out PlayerInfo? playerInfo, out var playerInfoErrors))
//         {
//             // TODO: finish
//         }
//         var statisticResponse = PlayerMapper.MapPlayerInfoToPlayerInfoResponse(playerInfo!);
//
//         return new AuthenticateResponse(
//             PlayerId: user.Id,
//             PlayerName: user.UserName,
//             Email: user.Email,
//             Token: accessToken,
//             RefreshToken: user.RefreshToken,
//             PlayerInfo: statisticResponse);
//     }
// }