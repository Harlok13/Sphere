namespace Identity.Domain.Configurations;

public class JwtConfiguration
{
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public int Expire { get; init; }
    public string SecretKey { get; init; } = null!;
    public int TokenValidityInMinutes { get; init; } 
    public int RefreshTokenValidityInDays { get; init; } 
}