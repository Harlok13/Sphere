// using System.IdentityModel.Tokens.Jwt;
// using Identity.Application.Repositories;
// using Identity.Application.Services;
// using Identity.Contracts.Requests;
// using Identity.Contracts.Responses;
// using Mediator;
// using Microsoft.Extensions.Logging;
//
// namespace Identity.Application.Commands.RefreshToken;
//
// public class RefreshTokenHandler : ICommandHandler<RefreshTokenCommand, RefreshTokenResponse>
// {
//     private readonly ILogger<RefreshTokenHandler> _logger;
//     private readonly IJwtService _jwtService;
//     private readonly IApplicationUserRepository _applicationUserRepository;
//
//     public RefreshTokenHandler(
//         ILogger<RefreshTokenHandler> logger,
//         IJwtService jwtService,
//         IApplicationUserRepository applicationUserRepository)
//     {
//         _logger = logger;
//         _jwtService = jwtService;
//         _applicationUserRepository = applicationUserRepository;
//     }
//     
//     public async ValueTask<RefreshTokenResponse> Handle(RefreshTokenCommand command, CancellationToken cT)
//     {
//         command.Deconstruct(out RefreshTokenRequest request);
//
//         if (request is null)
//         {
//             throw new Exception();  // TODO: ex
//             // return BadRequest("Invalid client request");
//         }
//
//         var accessToken = request.AccessToken;
//         var refreshToken = request.RefreshToken;
//         var principal = _jwtService.GetPrincipalFromExpiredToken(accessToken);
//
//         if (principal == null)
//         {
//             // return BadRequest("Invalid access token or refresh token");
//             throw new Exception();  // TODO: ex
//         }
//
//         var username = principal.Identity.Name;
//         var user = await _applicationUserRepository.GetManagedUserByNameAsync(username, cT);
//
//         if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
//         {
//             // return BadRequest("Invalid access token or refresh token");
//             throw new Exception();  // TODO: ex
//         }
//
//         var newAccessToken = _jwtService.CreateToken(principal.Claims.ToArray());
//         var newRefreshToken = _jwtService.GenerateRefreshToken();
//
//         user.RefreshToken = newRefreshToken;
//         await _applicationUserRepository.UpdateManagedUserAsync(user, cT);
//         
//         // return new ObjectResult(new
//         // {
//         //     accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
//         //     refreshToken = newRefreshToken
//         // });
//         return new RefreshTokenResponse(
//             new JwtSecurityTokenHandler().WriteToken(newAccessToken),
//             newRefreshToken);
//     }
// }