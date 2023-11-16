using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Sphere.DAL.Models;
using Sphere.Exceptions.JwtExceptions;
using Sphere.Services.Interfaces;

namespace Sphere.Services;

public class JwtService : IJwtService
{
    private readonly string _audience;
    private readonly string _issuer;
    private readonly string _secretKey;
    private readonly int _expire;
    private readonly int _tokenValidityInMinutes;

    private readonly ILogger<JwtService> _logger;

    public int RefreshTokenValidityInDays { get; }

    public JwtService(IConfiguration configuration, ILogger<JwtService> logger) 
    {
        _logger = logger;

        _audience = configuration["Jwt:Audience"] 
                    ?? throw new JwtReadConfigurationException(JwtConfigMsgEnum.AudienceError); 
        
        _issuer = configuration["Jwt:Issuer"] 
                  ?? throw new JwtReadConfigurationException(JwtConfigMsgEnum.IssuerError); 
        
        _secretKey = configuration["Jwt:Secret_SecretKey"] 
                     ?? throw new JwtReadConfigurationException(JwtConfigMsgEnum.SecretKeyError); 

        var expireSection = configuration.GetSection("Jwt:Secret_Expire") ??
                            throw new JwtReadConfigurationException(JwtConfigMsgEnum.ExpireError); 
        _expire = expireSection.Get<int>();

        if (_expire < 1) throw new JwtValueException(JwtValueMsgEnum.ExpireError); 

        var tokenValidityInMinutesSection = configuration.GetSection("Jwt:Secret_TokenValidityInMinutes")
            ?? throw new JwtReadConfigurationException(JwtConfigMsgEnum.TokenValidityInMinutesError);
        _tokenValidityInMinutes = tokenValidityInMinutesSection.Get<int>();

        if (_tokenValidityInMinutes < 1)
            throw new JwtValueException(JwtValueMsgEnum.TokenValidityInMinutesError); 

        var refreshTokenValidityInDaysSection = configuration.GetSection("Jwt:Secret_RefreshTokenValidityInDays")
                                                ?? throw new JwtReadConfigurationException(
                                                    JwtConfigMsgEnum.RefreshTokenValidityInDaysError
                                                ); 
        RefreshTokenValidityInDays = refreshTokenValidityInDaysSection.Get<int>();

        if (RefreshTokenValidityInDays < 1) throw new JwtValueException(JwtValueMsgEnum.RefreshTokenValidityInDaysError); 

        _logger.LogInformation(_expire.ToString());
        _logger.LogInformation(_tokenValidityInMinutes.ToString());
    }

    public string GetJwtToken(ApplicationUser user, IEnumerable<IdentityRole<int>> roles)
    {
        return new JwtSecurityTokenHandler().WriteToken(
            CreateJwtToken(CreateClaims(user, roles))
        );
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

    private List<Claim> CreateClaims(ApplicationUser user, IEnumerable<IdentityRole<int>> roles)
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