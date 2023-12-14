using App.Application.Identity;
using App.Application.SignalR.Hubs;
using App.Infra;
// using App.Infrastructure;
// using App.Infrastructure;
using Sphere.Exceptions.CorsExceptions;
using Sphere.SignalR;
using Sphere.SignalR.Hubs;

var builder = WebApplication.CreateBuilder(
    new WebApplicationOptions { Args = args, ContentRootPath = Directory.GetCurrentDirectory()});

builder.Configuration.AddJsonFile("appsettings.Secrets.json");

builder.Services.AddControllers();

// builder.Services.AddAuthenticationWithOptions(builder);
//
// builder.Services.AddAuthorizationWithOptions();
//
// builder.Services.AddIdentityWithOptions();
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
    
// builder.Services.AddScoped<IUserStatisticDbApi, UserStatisticDbApi>();
// builder.Services.AddScoped<IUserHistoryDbApi, UserHistoryDbApi>();
// builder.Services.AddScoped<IAuthDbApi, AuthDbApi>();
// builder.Services.AddScoped<ILobbyDbApi, LobbyDbApi>();
// builder.Services.AddScoped<IUserInfoDbApi, UserInfoDbApi>();

// builder.Services.AddInfrastructure();

// builder.Services.AddScoped<IGame21Service, Game21Service>();
// builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.AddIdentityServices();
// builder.Services.AddTransient<ICacheService, RedisService>();

// builder.Services.AddSingleton<ICardDeck, CardDeck>();
// builder.Services.AddSingleton<IGame, Game>();
// builder.Services.AddSingleton<IGameOnline, GameOnline>();
// builder.Services.AddTransient<ILevelComputed, LevelComputed>();
// builder.Services.AddApplicationDbContext(builder);

// builder.Services.AddScoped<IGlobalHubContext, GlobalHubContext>();


var app = builder.Build();
// app.ConfigurePipeline();

if (!app.Environment.IsDevelopment()) app.UseHsts();

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors(builder.Configuration["CorsPolicy"]
            ?? throw new CorsPolicyException("Current CORS Policy is not set."));  // TODO: use custom ex

app.UseAuthentication();
app.UseAuthorization();

// app.MapHub<RoomHub>("/room");
app.MapHub<GlobalHub>("/hubs/global");
// GlobalHub

app.MapControllers();

await app.RunAsync();