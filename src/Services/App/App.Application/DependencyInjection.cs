using App.Application.Services;
using App.Application.Services.Interfaces;
using App.GrpcClient.IdentityClient;
using App.GrpcClient.UserInteractionClient;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace App.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddScoped<ICardsDeckService, CardsDeckService>()
            .AddScoped<IUserInteractionClient, App.GrpcClient.UserInteractionClient.UserInteractionClient>()
            .AddScoped<IIdentityClient, App.GrpcClient.IdentityClient.IdentityClient>();
            // .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

        services.AddValidatorsFromAssembly(
            typeof(IApplicationAssemblyMarker).Assembly,
            includeInternalTypes: true);
        // services
        //     .Decorate<IRoomRepository, RoomRepositoryNotifyDecorator>();
        return services;
    }
}