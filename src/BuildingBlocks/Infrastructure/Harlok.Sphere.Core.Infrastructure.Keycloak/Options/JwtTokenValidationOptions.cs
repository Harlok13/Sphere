namespace Harlok.Sphere.Core.Infrastructure.Keycloak.Options;

/// <summary>
///     Jwt token validation configuration.
/// </summary>
public class JwtTokenValidationOptions
{
    /// <summary>
    ///     Check the publisher of the jwt token.
    /// </summary>
    public bool ValidateIssuer { get; init; }

    /// <summary>
    ///     Check who the jwt token was issued for.
    /// </summary>
    public bool ValidateAudience { get; init; }

    /// <summary>
    ///     Check jwt token lifetime.
    /// </summary>
    public bool ValidateLifetime { get; init; }

    /// <summary>
    ///     Publisher of jwt token.
    /// </summary>
    public string ValidIssuer { get; init; } = null!;
}