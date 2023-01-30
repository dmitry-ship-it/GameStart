using GameStart.CatalogService.Api.Extensions;
using GameStart.CatalogService.Common;
using GameStart.Shared.Extensions;
using GameStart.Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UsePreconfiguredSerilog(builder.Configuration);

builder.Services.AddAutoMapper(typeof(VideoGameManager));
builder.Services.AddDbContextsWithRepositories(builder.Configuration);
builder.Services.AddPreconfiguredJwtAuthentication(builder.Configuration);
builder.Services.AddControllersWithJsonOptions();

var app = builder.Build();

app.UseMiddleware<ExceptionLoggerMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
