using Microsoft.AspNetCore.Identity;

namespace App.Domain.Identity.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    // public ApplicationUser(string refreshToken, DateTime refreshTokenExpiryTime)
    // {
    //     RefreshToken = refreshToken;
    //     RefreshTokenExpiryTime = refreshTokenExpiryTime;
    // }
    // public string AvatarUrl { get; set; }  // TODO: finish
    
    public string? RefreshToken { get; set; }  // TODO: private?

    public DateTime RefreshTokenExpiryTime { get; set; }  // TODO: private?
}