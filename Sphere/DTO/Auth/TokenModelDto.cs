namespace Sphere.DTO.Auth;

public class TokenModelDto
{
    public string? AccessToken { get; set; }
    
    public string? RefreshToken { get; set; }
}