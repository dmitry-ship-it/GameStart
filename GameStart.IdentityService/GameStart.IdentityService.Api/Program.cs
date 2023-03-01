using GameStart.IdentityService.Api.Extensions;
using GameStart.IdentityService.Common.Mapping;
using GameStart.Shared.Extensions;
using GameStart.Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UsePreconfiguredSerilog(builder.Configuration);

builder.Services.AddDbContextsWithIdentity();
builder.Services.AddAutoMapper(typeof(UserProfile));
builder.Services.AddMassTransitEventConsuming();
builder.Services.AddControllersWithFilters();
builder.Services.AddPreconfiguredIdentityServer();
builder.Services.AddGoogleAuthentication(builder.Configuration);
builder.Services.AddCustomCorsPolicy();
builder.Services.AddManagersAndRepositories();

var app = builder.Build();

app.UseMiddleware<ExceptionLoggerMiddleware>();
app.UseHttpsRedirection();
app.UseMiddleware<CookieToHeaderWriterMiddleware>();
app.UseUpdateIdentityDbTables(app.Configuration);
app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
