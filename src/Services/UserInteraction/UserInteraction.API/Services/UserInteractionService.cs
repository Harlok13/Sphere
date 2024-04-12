using Grpc.Core;
using UserInteractionService;

namespace UserInteraction.API.Services;

public class UserInteractionService : global::UserInteractionService.UserInteraction.UserInteractionBase
{
    // private readonly ILogger<UserInteractionService> _logger;
    //
    // public UserInteractionService(ILogger<UserInteractionService> logger)
    // {
    //     _logger = logger;
    // }

    public override Task<AddToFriendsResponse> AddToFriends(AddToFriendsRequest request, ServerCallContext context)
    {
        return Task.FromResult(new AddToFriendsResponse
        {
            Name = request.Name
        });
    }
}