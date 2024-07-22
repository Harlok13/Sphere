using Keycloak.AuthServices.Authentication;
using Ocelot.Administration;
using Ocelot.DependencyInjection;

namespace Harlok.Sphere.ApiGateway.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOcelotWithAdministration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        KeycloakAuthenticationOptions keycloakOptions = configuration
            .GetSection("Keycloak")
            .Get<KeycloakAuthenticationOptions>() ?? throw new Exception("");

        services
            .AddOcelot()
            .AddAdministration("/administration", options =>
            {
                options.Authority = keycloakOptions.AuthServerUrl;
            });
        
        return services;
    }

    public static IServiceCollection AddCorsWithOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("DefaultPolicy", corsBuilder => corsBuilder
                .WithOrigins(configuration.GetOrigins())
                .AllowCredentials()
                .AllowAnyHeader()
                .AllowAnyMethod());
        });
        
        return services;
    }
}
