using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using App.Application.Identity.Exceptions;
using App.Application.Identity.Extensions;
using App.Domain.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace App.Application.Identity.Services;

public class JwtService : IJwtService
{
    private readonly string _audience;
    private readonly string _issuer;
    private readonly string _secretKey;
    private readonly int _expire;
    private readonly int _tokenValidityInMinutes;
    private readonly int _refreshTokenValidityInDays;

    private readonly ILogger<JwtService> _logger;

    public int RefreshTokenValidityInDays => _refreshTokenValidityInDays;

    public JwtService(IConfiguration configuration, ILogger<JwtService> logger) 
    {
        _logger = logger;

        _audience = configuration.GetAudience();
        _issuer = configuration.GetIssuer();
        _secretKey = configuration.GetSecretKey();
        _expire = configuration.GetExpire();
        _tokenValidityInMinutes = configuration.GetTokenValidityInMinutes();
        _refreshTokenValidityInDays = configuration.GetRefreshTokenValidityInDays(); 

        _logger.LogInformation(_expire.ToString());
        _logger.LogInformation(_tokenValidityInMinutes.ToString());
    }

    public string GetJwtToken(ApplicationUser user, IEnumerable<IdentityRole<Guid>> roles)
    {
        return new JwtSecurityTokenHandler().WriteToken(
            CreateJwtToken(CreateClaims(user, roles))
        );
    }

    public string GetJwtToken(ApplicationUser user, IEnumerable<IdentityRole> roles)
    {
        throw new NotImplementedException();
    }

    public JwtSecurityToken CreateToken(IEnumerable<Claim> authClaims) 
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

        return new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            expires: DateTime.UtcNow.AddMinutes(_tokenValidityInMinutes),
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
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
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
            SecurityAlgorithms.HmacSha256
        );
    }

    private List<Claim> CreateClaims(ApplicationUser user, IEnumerable<IdentityRole<Guid>> roles)
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
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_expire),
            signingCredentials: CreateSigningCredentials()
        );
    }
}