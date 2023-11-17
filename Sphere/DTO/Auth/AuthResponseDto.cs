using Sphere.DTO.Game21;

namespace Sphere.DTO.Auth;

public class AuthResponseDto
{
    public int UserId { get; set; }
    
    public string UserName { get; set; } = null!;
    
    public string Email { get; set; } = null!;
    
    public string Token { get; set; } = null!;
    
    public string RefreshToken { get; set; } = null!;
    
    public UserStatisticDto UserStatistic { get; set; } = null!;
}