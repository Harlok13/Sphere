using Harlok.Sphere.ApiGateway.API.Extensions;
using Harlok.Sphere.Core.Extensions;
using Harlok.Sphere.Core.Infrastructure.Keycloak;
using Harlok.Sphere.Core.Serilog;
using Ocelot.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// builder.Configuration.AddCoreJsonConfiguration(builder.Environment);
builder.Configuration.AddConfigurationByName("keycloak", builder.Environment);

builder.Services.AddKeycloak(builder.Configuration, builder.Environment);
builder.Services.AddOcelotWithAdministration(builder.Configuration);
builder.Services.AddCorsWithOptions(builder.Configuration);
builder.Host.UseSerilogWithConfig();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
await app.UseOcelot();

app.Run();


