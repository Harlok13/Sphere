// using Identity.Application.Repositories;
// using Mediator;
// using Microsoft.Extensions.Logging;
//
// namespace Identity.Application.Commands.RevokeAll;
//
// public class RevokeAllHandler : ICommandHandler<RevokeAllCommand, bool>
// {
//     private readonly ILogger<RevokeAllHandler> _logger;
//     private readonly IApplicationUserRepository _applicationUserRepository;
//
//     public RevokeAllHandler(
//         ILogger<RevokeAllHandler> logger,
//         IApplicationUserRepository applicationUserRepository)
//     {
//         _logger = logger;
//         _applicationUserRepository = applicationUserRepository;
//     }
//     
//     public async ValueTask<bool> Handle(RevokeAllCommand command, CancellationToken cT)
//     {
//         var users = await _applicationUserRepository.GetAllManagedUsersAsync(cT);
//         foreach (var user in users)
//         {
//             user.RefreshToken = null;
//             await _applicationUserRepository.UpdateManagedUserAsync(user, cT);
//         }
//
//         return true;
//     }
// }