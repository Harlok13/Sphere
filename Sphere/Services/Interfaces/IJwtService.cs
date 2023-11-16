using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Sphere.DAL.Models;

namespace Sphere.Services.Interfaces;

public interface IJwtService
{
    public string GetJwtToken(ApplicationUser user, IEnumerable<IdentityRole<int>> roles);

    public JwtSecurityToken CreateToken(IEnumerable<Claim> authClaims);

    public string GenerateRefreshToken();

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
    
    public int RefreshTokenValidityInDays { get; }
}