using Identity.API.Extensions;
using Identity.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// builder.Services.AddControllers();
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

builder.Services.AddGrpc();

builder.Services.AddConfigurations(builder.Configuration);

builder.Services.AddConfiguredPipeline(builder);

var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();

// app.UseAuthorization();
app.UseAuthentication();
app.UseAuthorization();
// app.MapControllers();

app.MapGrpcService<Identity.API.Services.IdentityService>();

app.Run();