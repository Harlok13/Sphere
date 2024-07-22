namespace App.Contracts.Requests;

public sealed record AddToFriendsRequest(
    Guid PlayerId,
    Guid FriendId);