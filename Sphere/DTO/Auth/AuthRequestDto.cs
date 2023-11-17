namespace Sphere.DTO.Auth;

public class AuthRequestDto
{
    public string Email { get; init; } = null!;
    
    public string Password { get; init; } = null!;
}