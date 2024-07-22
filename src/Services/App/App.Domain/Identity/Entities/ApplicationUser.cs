using Microsoft.AspNetCore.Identity;

namespace App.Domain.Identity.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? RefreshToken { get; set; }  

    public DateTime RefreshTokenExpiryTime { get; set; }  
}