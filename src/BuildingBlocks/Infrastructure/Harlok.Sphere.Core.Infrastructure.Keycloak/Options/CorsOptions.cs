namespace Harlok.Sphere.Core.Infrastructure.Keycloak.Options;

public class CorsOptions
{
    public string[] AllowedOrigins { get; init; } = null!;

    public bool AllowCredentials { get; init; }

    public string[] AllowedHeaders { get; init; }

    public string[] AllowedMethods { get; init; }
}