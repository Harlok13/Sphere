using Identity.Application;
using Identity.Application.Configurations;
using Identity.Application.Repositories;
using Identity.Application.Repositories.UnitOfWork;
using Identity.Infrastructure;
using Identity.Infrastructure.Repositories;
using Scrutor;

namespace Identity.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfiguredPipeline(this IServiceCollection services, IConfiguration configuration)
    {
        // services.Scan(scan => scan
        //     .FromAssemblies(
        //         typeof(IApplicationAssemblyMarker).Assembly,
        //         typeof(IInfrastructureAssemblyMarker).Assembly)
        //     .AddClasses(classes =>
        //         classes.Where(type => type.Name.EndsWith("Repository") || type.Name.EndsWith("Work")))
        //     .UsingRegistrationStrategy(RegistrationStrategy.Skip)
        //     .AsImplementedInterfaces()
        //     .WithScopedLifetime());

        services.AddScoped<IIdentityUnitOfWork, IdentityUnitOfWork>()
            .AddScoped<IIdentityUserRepository, IdentityUserRepository>();

        services
            .AddIdentityInfrastructure(configuration)
            .AddApplication();
        
        return services;
    }
    
    public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtConfiguration>(configuration.GetSection("JwtConfiguration"));
        
        return services;
    }
    
    // public static IServiceCollection AddMediatorWithOptions(this IServiceCollection services)
    // {
    //     return services.AddMediator(options =>
    //         options.ServiceLifetime = ServiceLifetime.Transient);
    // }
}