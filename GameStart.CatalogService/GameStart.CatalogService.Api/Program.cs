using GameStart.CatalogService.Api.Extensions;
using GameStart.Shared.Extensions;
using GameStart.Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UsePreconfiguredSerilog(builder.Configuration);

builder.Services.AddDbContextWithRepositories(builder.Configuration);
builder.Services.AddModelsMapper();
builder.Services.AddPreconfiguredJwtAuthentication(builder.Configuration);
builder.Services.AddControllersWithJsonConfiguration();

var app = builder.Build();

app.UseMiddleware<ExceptionLoggerMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
