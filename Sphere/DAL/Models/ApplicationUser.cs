using Microsoft.AspNetCore.Identity;

namespace Sphere.DAL.Models;

public class ApplicationUser : IdentityUser<int>
{
    public string? RefreshToken { get; set; }
    
    public DateTime RefreshTokenExpiryTime { get; set; }
}