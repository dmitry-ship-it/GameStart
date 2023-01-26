using GameStart.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UsePreconfiguredSerilog(builder.Configuration);



var app = builder.Build();

app.Run();
