using UserInteraction.Application;
using UserInteraction.Application.Repositories;
using UserInteraction.Application.Repositories.UnitOfWork;
using UserInteraction.Infrastructure;
using UserInteraction.Infrastructure.Repositories;

namespace UserInteraction.API.Extensions;

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

        services
            .AddScoped<IUserInteractionUnitOfWork, UserInteractionUnitOfWork>()
            .AddScoped<IPlayerHistoryRepository, PlayerHistoryRepository>()
            .AddScoped<IPlayerHistoryRepository, PlayerHistoryRepository>();

        services
            .AddUserInteractionInfrastructure(configuration)
            .AddApplication();
        
        return services;
    }
    
    // public static IServiceCollection AddMediatorWithOptions(this IServiceCollection services)
    // {
    //     return services.AddMediator(options =>
    //         options.ServiceLifetime = ServiceLifetime.Transient);
    // }
}