using GameStart.CatalogService.Api.Extensions;
using GameStart.CatalogService.Data;
using GameStart.Shared.Extensions;
using GameStart.Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UsePreconfiguredSerilog(builder.Configuration);

builder.Services.AddDbContextWithRepositories();
builder.Services.AddRedisCache();
builder.Services.AddModelsMapper();
builder.Services.AddPreconfiguredJwtAuthentication();
builder.Services.AddMassTransitEventConsuming();
builder.Services.AddControllersWithJsonConfiguration();

var app = builder.Build();

app.UseMiddleware<ExceptionLoggerMiddleware>();
app.UseHttpsRedirection();
app.UseAutoCreatingForDatabases(typeof(CatalogDbContext));
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
