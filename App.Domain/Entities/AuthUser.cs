namespace App.Domain.Entities;


public class AuthUser : IUser
{
    private AuthUser(Guid id, string userName)
    {
        Id = id;
        UserName = userName;
    }

    private AuthUser(Guid id, string userName, string connectionId)
    {
        Id = id;
        UserName = userName;
        ConnectionId = connectionId;
    }

    public AuthUser() { }

    public Guid Id { get; set; }
    public string UserName { get; set; } = null!;
    public string? ConnectionId { get; set; }

    public static AuthUser Create(Guid id, string userName)
    {
        return new AuthUser(id, userName);
    }
    
    public static AuthUser Create(Guid id, string userName, string connectionId)
    {
        return new AuthUser(id, userName, connectionId);
    }
}