using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Services;

public interface IJwtService
{
    int RefreshTokenValidityInDays { get; }
    
    string GetJwtToken(ApplicationUser user, IEnumerable<IdentityRole<Guid>> roles);
    
    JwtSecurityToken CreateToken(IEnumerable<Claim> authClaims);

    string GenerateRefreshToken();

    ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
}