using Core.Extensions;
using Core.Serilog;
using UserInteraction.API.Extensions;
using UserInteraction.API.Mapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddConfiguredPipeline(builder.Configuration);

builder.Services.AddMediatorWithOptions();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Host.UseSerilogWithConfig();

var app = builder.Build();

app.MapGrpcService<UserInteraction.API.Services.UserInteractionService>();

app.Run();