using App.Application;
using App.Application.Identity;
using App.Infra;
using App.SignalR.Hubs;
using Scrutor;
using Serilog;
using Sphere;
using Sphere.Exceptions.CorsExceptions;

var builder = WebApplication.CreateBuilder(
    new WebApplicationOptions { Args = args, ContentRootPath = Directory.GetCurrentDirectory()});

builder.Configuration.AddJsonFile("appsettings.Secrets.json");

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
        .Enrich.With(new AbbreviatedSourceContextEnricher(50, 50)));

builder.Services.AddControllers();

builder.Services.AddInfrastructure(builder);

builder.Services.AddMediator(options => 
    options.ServiceLifetime = ServiceLifetime.Transient);


builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.CheckConsentNeeded = context => true;
    options.ConsentCookie.IsEssential = true;
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "local";
});

// builder.Services.AddDistributedMemoryCache();
// builder.Services.AddStackExchangeRedisCache(options =>
// {
//     options.Configuration = builder.Configuration.GetSection("Redis:Dsn").Get<string>()  // TODO: remove Get
//         ?? throw new InvalidOperationException("Connection string for redis is not set");  // TODO: custom ex
// });
// builder.Services.AddSession(options =>
// {
//     options.IdleTimeout = TimeSpan.FromMinutes(30);  // TODO: move to settings 
//     options.Cookie.HttpOnly = true;
//     options.Cookie.IsEssential = true;
// });

builder.Services.AddSignalR(
//     options =>
// {
//     options.DisableImplicitFromServicesParameters = true;
//     // options.
// }
    );

// builder.Services.AddCorsWithOptions(builder);
builder.Services.AddCors(options =>
    {
        options.AddPolicy("DevPolicy", corsBuilder => corsBuilder
            .WithOrigins(builder.Configuration.GetSection("Cors:DevUrls").Get<string[]>()  // TODO: check Get
                         ?? throw new CorsHostsException("Allowed hosts for CORS is not set."))  // TODO: custom ex
            .AllowCredentials()
            .AllowAnyHeader()
            .WithMethods("GET", "POST")
        );
    }
);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// builder.Services.Scan(scan => scan
//     .FromAssemblyOf<IInfrastructureAssemblyMarker>()
//     .AddClasses(classes =>
//         classes.Where(type => type.Name.EndsWith("Repository") || type.Name.EndsWith("Work")))
//     .UsingRegistrationStrategy(RegistrationStrategy.Skip)
//     .AsImplementedInterfaces()
//     .WithScopedLifetime());

builder.Services.Scan(scan => scan
    .FromAssemblies(
        typeof(IApplicationAssemblyMarker).Assembly,
        typeof(IInfrastructureAssemblyMarker).Assembly)
    .AddClasses(classes =>
        classes.Where(type => type.Name.EndsWith("Repository") || type.Name.EndsWith("Work")))
    .UsingRegistrationStrategy(RegistrationStrategy.Skip)
    .AsImplementedInterfaces()
    .WithScopedLifetime());


builder.Services
    .AddIdentityServices()
    .AddApplication();


    // .AddClasses(classes => classes
    //     .Where(type => type.Name.EndsWith("Work")))
    // .UsingRegistrationStrategy(RegistrationStrategy.Skip)
    // .AsImplementedInterfaces()
    // .WithScopedLifetime()
    //
    // .FromAssemblyOf<IApplicationAssemblyMarker>()
    // .AddClasses(classes => classes.Where(type => type.IsClass)));


var app = builder.Build();
// app.ConfigurePipeline();

if (!app.Environment.IsDevelopment()) app.UseHsts();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors(builder.Configuration["CorsPolicy"]
            ?? throw new CorsPolicyException("Current CORS Policy is not set."));  // TODO: use custom ex

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<GlobalHub>("/hubs/global");

app.MapControllers();

await app.RunAsync();
