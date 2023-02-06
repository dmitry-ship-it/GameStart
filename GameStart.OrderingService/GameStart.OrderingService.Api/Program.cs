using FluentValidation;
using GameStart.OrderingService.Api.Extensions;
using GameStart.OrderingService.Application.Mapping;
using GameStart.OrderingService.Application.Validators;
using GameStart.Shared.Extensions;
using GameStart.Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UsePreconfiguredSerilog(builder.Configuration);

builder.Services.AddDbContextWithRepositories(builder.Configuration);
builder.Services.AddAutoMapper(typeof(OrderProfile));
builder.Services.AddControllersWithJsonConfiguration();
builder.Services.AddValidatorsFromAssemblyContaining<OrderDtoValidator>();
builder.Services.AddCustomServices();
builder.Services.AddPreconfiguredJwtAuthentication(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionLoggerMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
