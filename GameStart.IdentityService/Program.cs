using GameStart.IdentityService.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextsWithUsers(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddPreconfiguredIdentityServer();
builder.Services.AddGoogleAuthentication(builder.Configuration);
builder.Services.AddCustomCorsPolicy();

var app = builder.Build();

app.UseHttpsRedirection();

app.UpdateIdentityDbTables(app.Configuration);
app.UseIdentityServer();
app.UseAuthorization();
app.MapControllers();

app.Run();
