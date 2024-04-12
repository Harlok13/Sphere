using Identity.Application;
using Identity.Domain.Configurations;
using Infrastructure;
using Scrutor;

namespace Identity.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfiguredPipeline(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.Scan(scan => scan
            .FromAssemblies(
                typeof(IApplicationAssemblyMarker).Assembly,
                typeof(IInfrastructureAssemblyMarker).Assembly)
            .AddClasses(classes =>
                classes.Where(type => type.Name.EndsWith("Repository") || type.Name.EndsWith("Work")))
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services
            // .AddInfrastructure(builder)
            .AddApplication();
        
        return services;
    }
    
    public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtConfiguration>(configuration.GetSection("JwtConfiguration"));
        
        return services;
    }
}