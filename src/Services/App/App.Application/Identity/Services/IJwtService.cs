using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using App.Domain.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace App.Application.Identity.Services;

public interface IJwtService
{
    int RefreshTokenValidityInDays { get; }
    
    string GetJwtToken(ApplicationUser user, IEnumerable<IdentityRole<Guid>> roles);
    
    JwtSecurityToken CreateToken(IEnumerable<Claim> authClaims);

    string GenerateRefreshToken();

    ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
}