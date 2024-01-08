using App.Infra;
using Scrutor;

namespace Sphere;

public static class ServiceRegistrations
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<IInfrastructureAssemblyMarker>()
            .AddClasses(classes => classes
                .Where(type => type.Name.EndsWith("Repository")))
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
    
    
    /*public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        //services
            //.AddServices(typeof(ITransientService), ServiceLifetime.Transient);
        return services;
    }
    
    internal static IServiceCollection AddServices(
        this IServiceCollection services,
        Type interfaceType,
        ServiceLifetime lifetime
    )
    {
        var interfaceTypes = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(t => interfaceType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
            .Select(t => new { Service = t.GetInterfaces().FirstOrDefault(), Implementation = t })
            .Where(t => t.Service is not null && interfaceType.IsAssignableFrom(t.Service));
    
        foreach (var type in interfaceTypes)
            services.AddService(type.Service!, type.Implementation, lifetime);
    
        return services;
    }
    
    internal static IServiceCollection AddService(
        this IServiceCollection services,
        Type serviceType,
        Type implementationType,
        ServiceLifetime lifetime
    )
    {
        return lifetime switch
        {
            ServiceLifetime.Transient => services.AddTransient(serviceType, implementationType),
            ServiceLifetime.Scoped => services.AddScoped(serviceType, implementationType),
            ServiceLifetime.Singleton => services.AddSingleton(serviceType, implementationType),
            _ => throw new ArgumentException("Invalid lifeTime", nameof(lifetime))
        };
    }*/

}