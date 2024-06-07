using Core.Extensions;
using Core.Serilog;
using Identity.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddConfigurations(builder.Configuration);

builder.Services.AddConfiguredPipeline(builder.Configuration);

builder.Services.AddMediatorWithOptions();

builder.Host.UseSerilogWithConfig();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<Identity.API.Services.IdentityService>();

app.Run();