using Microsoft.AspNetCore.Identity;

namespace Sphere.DAL.Models;

public class ApplicationUser : IdentityUser<int>
{
    public string Avatar { get; set; } = "/img/avatars/default_avatar.png";  // TODO: relocate to settings and add configurations
    
    public string? RefreshToken { get; set; }
    
    public DateTime RefreshTokenExpiryTime { get; set; }
}