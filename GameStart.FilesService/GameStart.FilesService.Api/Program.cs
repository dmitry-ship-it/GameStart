using GameStart.FilesService.Api.Extensions;
using GameStart.Shared.Extensions;
using GameStart.Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UsePreconfiguredSerilog(builder.Configuration);
builder.Services.AddAuthenticationWithJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddCustomServices();
builder.Services.AddControllersWithFilters();

var app = builder.Build();

app.UseMiddleware<ExceptionLoggerMiddleware>();
app.UseMiddleware<CookieToHeaderWriterMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
