using GameStart.Gateway.Extensions;
using GameStart.Shared.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UsePreconfiguredSerilog(builder.Configuration);

builder.Configuration.AddOcelotConfiguration(builder.Environment);
builder.Services.AddOcelot();

var app = builder.Build();

await app.UseOcelot();

app.Run();
