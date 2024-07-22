namespace Harlok.Sphere.ApiGateway.API.Extensions;

/// <summary>
///     Extension methods for <see cref="IConfiguration"/>
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    ///     Get allowed origins from configuration.
    /// </summary>
    /// <param name="configuration">Configuration properties.</param>
    /// <returns>The collection of origins.</returns>
    /// <exception cref="Exception">Allowed hosts for CORS is not set.</exception>
    public static string[] GetOrigins(this IConfiguration configuration)
    {
        string[] urls = configuration.GetSection("Cors:Urls").Get<string[]>()
                        ?? throw new Exception("Allowed hosts for CORS is not set.");

        return urls;
    }
}