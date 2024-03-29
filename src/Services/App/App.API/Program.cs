using App.API.Extensions;
using App.SignalR.Hubs;
using Serilog;

var builder = WebApplication.CreateBuilder(
    new WebApplicationOptions { Args = args, ContentRootPath = Directory.GetCurrentDirectory()});

builder.Configuration.AddJsonFile("appsettings.Secrets.json");

builder.Host.UseSerilogWithConfig();

builder.Services.AddControllers();

builder.Services.AddConfiguredPipeline(builder);

builder.Services.ConfigureCookiePolicy();

builder.Services
    .AddMediatorWithOptions()
    .AddStackExchangeRedisCacheWithOptions(builder)
    .AddSignalRWithOptions(builder)
    .AddCorsWithOptions(builder);

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (!app.Environment.IsDevelopment()) app.UseHsts();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors(builder.Configuration.GetCorsPolicy());

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<GlobalHub>("/hubs/global");

app.MapControllers();

await app.RunAsync();
