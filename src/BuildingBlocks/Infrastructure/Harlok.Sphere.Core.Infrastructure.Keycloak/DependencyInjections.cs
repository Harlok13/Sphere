using Harlok.Sphere.Core.Infrastructure.Keycloak.Options;
using Keycloak.AuthServices.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Harlok.Sphere.Core.Infrastructure.Keycloak;

/// <summary>
///     Register dependencies in di container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Add keycloak settings.
    /// </summary>
    /// <param name="services">Collection of services.</param>
    /// <param name="configuration">Configuration properties.</param>
    /// <param name="environment">Provides information about the hosting environment an application is running in.</param>
    /// <returns>Collection of services.</returns>
    /// <exception cref="Exception">Settings for jwt token or keycloak not found.</exception>
    public static IServiceCollection AddKeycloak(this IServiceCollection services, 
        IConfiguration configuration, IHostEnvironment environment)
    {
        KeycloakAuthenticationOptions keycloakOptions = configuration
            .GetSection("Keycloak")
            .Get<KeycloakAuthenticationOptions>()
            ?? throw new Exception("Keycloak settings not found.");

        JwtTokenValidationOptions jwtTokenValidationOptions = configuration
            .GetSection("JwtTokenValidation")
            .Get<JwtTokenValidationOptions>()
            ?? throw new Exception("Jwt token validation settings not found.");
        
        services.AddKeycloakWebApiAuthentication(options =>
        {
            options.Audience = keycloakOptions.Audience;
            options.Realm = keycloakOptions.Realm;
            options.VerifyTokenAudience = keycloakOptions.VerifyTokenAudience;
            options.AuthServerUrl = keycloakOptions.AuthServerUrl;
        }, options =>
        {
            // settings for authentication in SignalR
            options.Events = new JwtBearerEvents()
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];

                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                }
            };
            
            // additional validation settings
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = jwtTokenValidationOptions.ValidateIssuer,
                ValidateAudience = jwtTokenValidationOptions.ValidateAudience,
                ValidateLifetime = jwtTokenValidationOptions.ValidateLifetime,
                ValidIssuer = jwtTokenValidationOptions.ValidIssuer,
                ValidAudience = keycloakOptions.Audience,
            };

            // disable https during development
            options.RequireHttpsMetadata = false;
            if (environment.IsDevelopment())
            {
            }
        }, "CustomBearerScheme");
        
        return services;
    }
}