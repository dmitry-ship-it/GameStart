using GameStart.OrderingService.Api.Extensions;
using GameStart.OrderingService.Application.Mapping;
using GameStart.Shared.Extensions;
using GameStart.Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UsePreconfiguredSerilog(builder.Configuration);

builder.Services.AddDbContextWithRepositories(builder.Configuration);
builder.Services.AddAutoMapper(typeof(OrderProfile));
builder.Services.AddControllersWithJsonConfiguration();

var app = builder.Build();

app.UseMiddleware<ExceptionLoggerMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();