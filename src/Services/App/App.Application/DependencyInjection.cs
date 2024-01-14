using App.Application.Services;
using App.Application.Services.Interfaces;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace App.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddScoped<ICardsDeckService, CardsDeckService>();
            // .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

        services.AddValidatorsFromAssembly(
            typeof(IApplicationAssemblyMarker).Assembly,
            includeInternalTypes: true);
        // services
        //     .Decorate<IRoomRepository, RoomRepositoryNotifyDecorator>();
        return services;
    }
}