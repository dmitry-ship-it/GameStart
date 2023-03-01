using GameStart.MailingService.Api.Extensions;
using GameStart.Shared.Extensions;
using GameStart.Shared.Middlewares;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UsePreconfiguredSerilog(builder.Configuration);
builder.Configuration.AddEmailConfigurationFile(builder.Environment);

builder.Services.AddServices();
builder.Services.AddEmailConfiguration(builder.Configuration);
builder.Services.AddPreconfiguredHangfire();
builder.Services.AddMassTransitEventConsuming();

var app = builder.Build();

app.UseMiddleware<ExceptionLoggerMiddleware>();
app.UseHttpsRedirection();
app.UseHangfireDashboard();

app.Run();
