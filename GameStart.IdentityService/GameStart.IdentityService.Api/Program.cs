using GameStart.IdentityService.Common;
using GameStart.IdentityService.Data.Mapping;
using GameStart.IdentityService.Api.Extensions;
using GameStart.Shared.Middlewares;
using GameStart.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UsePreconfiguredSerilog(builder.Configuration);

builder.Services.AddDbContextsWithIdentity();
builder.Services.AddAutoMapper(typeof(UserProfile));
builder.Services.AddMassTransitEventConsuming();
builder.Services.AddControllers();
builder.Services.AddPreconfiguredIdentityServer();
builder.Services.AddGoogleAuthentication(builder.Configuration);
builder.Services.AddCustomCorsPolicy();
builder.Services.AddScoped<AccountManager>();

var app = builder.Build();

app.UseMiddleware<ExceptionLoggerMiddleware>();
app.UseHttpsRedirection();
app.UseUpdateIdentityDbTables(app.Configuration);
app.UseIdentityServer();
app.UseAuthorization();
app.MapControllers();

app.Run();
