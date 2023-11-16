using Sphere.BL.Game21;
using Sphere.BL.Game21.Interfaces;
using Sphere.BL.Game21.Online;
using Sphere.DAL.Auth;
using Sphere.DAL.Game21;
using Sphere.Exceptions.CorsExceptions;
using Sphere.Extensions;
using Sphere.Hubs;
using Sphere.Services;
using Sphere.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.Secrets.json");

builder.Services.AddControllers();

builder.Services.AddAuthenticationWithOptions(builder);

builder.Services.AddAuthorizationWithOptions();

builder.Services.AddIdentityWithOptions();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.CheckConsentNeeded = context => true;
    options.ConsentCookie.IsEssential = true;
});

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
    {
        options.AddPolicy("DevPolicy", corsBuilder => corsBuilder
            .WithOrigins(builder.Configuration.GetSection("Cors:DevUrls").Get<string[]>()  
                         ?? throw new CorsHostsException("Allowed hosts for CORS is not set."))  
            .AllowCredentials()
            .AllowAnyHeader()
            .WithMethods("GET", "POST")
        );
    }
);

builder.Services.AddApplicationDbContext(builder);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    
builder.Services.AddScoped<IUserStatisticDbApi, UserStatisticDbApi>();
builder.Services.AddScoped<IUserHistoryDbApi, UserHistoryDbApi>();
builder.Services.AddScoped<IAuthDbApi, AuthDbApi>();
    
builder.Services.AddScoped<IGame21Service, Game21Service>();
builder.Services.AddSingleton<IJwtService, JwtService>();

builder.Services.AddSingleton<ICardDeck, CardDeck>();
builder.Services.AddSingleton<IGame, Game>();
builder.Services.AddSingleton<IGameOnline, GameOnline>();
builder.Services.AddTransient<ILevelComputed, LevelComputed>();


var app = builder.Build();

if (!app.Environment.IsDevelopment()) app.UseHsts();

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors(builder.Configuration["CorsPolicy"]
            ?? throw new CorsPolicyException("Current CORS Policy is not set."));  

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<RoomHub>("/room");

app.MapControllers();

await app.RunAsync();