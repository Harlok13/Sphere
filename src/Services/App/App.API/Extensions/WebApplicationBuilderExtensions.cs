using Serilog;

namespace App.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static IHostBuilder UseSerilogWithConfig(this IHostBuilder builder)
    {
        return builder.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration)
                .Enrich.With(new AbbreviatedSourceContextEnricher(50, 50)));
    }
}