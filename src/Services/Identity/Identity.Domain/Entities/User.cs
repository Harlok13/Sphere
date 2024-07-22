using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public string? RefreshToken { get; set; }  

    public DateTime RefreshTokenExpiryTime { get; set; }  
}