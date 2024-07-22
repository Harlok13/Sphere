using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Harlok.Sphere.Core.Extensions;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddConfigurationByName(
        this IConfigurationBuilder builder,
        string name,
        IHostEnvironment environment)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile(
                path: $"{name}.{environment.EnvironmentName}.json",
                optional: false,
                reloadOnChange: true)
            .Build();

        builder.AddConfiguration(configuration);

        return builder;
    }
}