using Microsoft.Extensions.DependencyInjection;

namespace Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediatorWithOptions(this IServiceCollection services)
    {
        return services.AddMediator(options =>
            options.ServiceLifetime = ServiceLifetime.Transient);
    }
}