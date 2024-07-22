using Core.Extensions;
using Core.Serilog;
using GameInteraction.API.Mapper;
using GameInteraction.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddMediatorWithOptions();

builder.Host.UseSerilogWithConfig();

var app = builder.Build();

app.MapGrpcService<GameInteractionService>();

app.Run();