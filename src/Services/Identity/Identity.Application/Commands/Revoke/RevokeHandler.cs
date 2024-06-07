// using Identity.Application.Repositories;
// using Mediator;
// using Microsoft.Extensions.Logging;
//
// namespace Identity.Application.Commands.Revoke;
//
// public class RevokeHandler : ICommandHandler<RevokeCommand, bool>
// {
//     private readonly ILogger<RevokeHandler> _logger;
//     private readonly IIdentityUserRepository _applicationUserRepository;
//
//     public RevokeHandler(
//         ILogger<RevokeHandler> logger,
//         IIdentityUserRepository applicationUserRepository)
//     {
//         _logger = logger;
//         _applicationUserRepository = applicationUserRepository;
//     }
//     
//     public async ValueTask<bool> Handle(RevokeCommand command, CancellationToken cT)
//     {
//         var user = await _applicationUserRepository.GetManagedUserByNameAsync(command.UserName, cT);
//         if (user is null) return false;
//
//         user.RefreshToken = null;
//         await _applicationUserRepository.UpdateManagedUserAsync(user, cT);
//
//         return true;
//     }
// }