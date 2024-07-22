using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Identity.Application.Configurations;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Application.Services;

public class JwtService : IJwtService
{
    private readonly JwtConfiguration _jwtConfiguration;

    private readonly ILogger<JwtService> _logger;

    public int RefreshTokenValidityInDays => _jwtConfiguration.RefreshTokenValidityInDays;

    public JwtService(
        ILogger<JwtService> logger,
        IOptions<JwtConfiguration> jwtConfiguration) 
    {
        _logger = logger;
        _jwtConfiguration = jwtConfiguration.Value;
    }

    public string GetJwtToken(User user, IEnumerable<IdentityRole<Guid>> roles)
        => new JwtSecurityTokenHandler().WriteToken(CreateJwtToken(CreateClaims(user, roles)));

    public JwtSecurityToken CreateToken(IEnumerable<Claim> authClaims) 
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey));

        return new JwtSecurityToken(
            issuer: _jwtConfiguration.Issuer,
            audience: _jwtConfiguration.Audience,
            expires: DateTime.UtcNow.AddMinutes(_jwtConfiguration.TokenValidityInMinutes),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var range = RandomNumberGenerator.Create();

        range.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken
            || !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token.");

        return principal;
    }

    private SigningCredentials CreateSigningCredentials()
    {
        return new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey)),
            SecurityAlgorithms.HmacSha256
        );
    }

    private List<Claim> CreateClaims(User user, IEnumerable<IdentityRole<Guid>> roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, string.Join(" ", roles.Select(x => x.Name)))
        };

        return claims;
    }

    private JwtSecurityToken CreateJwtToken(IEnumerable<Claim> claims)
    {
        return new JwtSecurityToken(
            issuer: _jwtConfiguration.Issuer,
            audience: _jwtConfiguration.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtConfiguration.TokenValidityInMinutes),
            signingCredentials: CreateSigningCredentials()
        );
    }
}