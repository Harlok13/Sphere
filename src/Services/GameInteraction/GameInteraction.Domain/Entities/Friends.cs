namespace GameInteraction.Domain.Entities;

public sealed class Friends
{
    // private Friends(
    //     Guid playerId,
    //     Guid friendId)
    // {
    //     PlayerInfoId = playerId;
    //     FriendsId = friendId;
    // }
    
    public Guid FriendsId { get; private set; }
    public Guid PlayerInfoId { get; private set; }

    public static Friends Create(Guid playerId, Guid friendId)
    {
        var friends = new Friends();
        friends.FriendsId = friendId;
        friends.PlayerInfoId = playerId;

        return friends;
    }
}