using GameStart.IdentityService.Api.Extensions;
using GameStart.IdentityService.Common.Mapping;
using GameStart.Shared.Extensions;
using GameStart.Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UsePreconfiguredSerilog(builder.Configuration);

builder.Services.AddDbContextsWithIdentity();
builder.Services.AddAutoMapper(typeof(UserProfile));
builder.Services.AddMassTransitEventConsuming();
builder.Services.AddControllers();
builder.Services.AddPreconfiguredIdentityServer();
builder.Services.AddGoogleAuthentication(builder.Configuration);
builder.Services.AddCustomCorsPolicy();
builder.Services.AddManagers();

var app = builder.Build();

app.UseMiddleware<ExceptionLoggerMiddleware>();
app.UseHttpsRedirection();
app.UseUpdateIdentityDbTables(app.Configuration);
app.UseIdentityServer();
app.UseAuthorization();
app.MapControllers();

app.Run();
