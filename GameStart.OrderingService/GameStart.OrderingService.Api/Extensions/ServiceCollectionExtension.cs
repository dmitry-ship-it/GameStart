using GameStart.OrderingService.Application.Services;
using GameStart.OrderingService.Core.Abstractions;
using GameStart.OrderingService.Infrastructure;
using GameStart.OrderingService.Infrastructure.Repositories;
using GameStart.Shared;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace GameStart.OrderingService.Api.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDbContextWithRepositories(this IServiceCollection services)
        {
            services.AddDbContext<OrdersDbContext>(options =>
                options.UseMySQL(Constants.OrderingService.ConnectionStrings.OrdersDb, config =>
                    config.MigrationsAssembly(typeof(OrdersDbContext).Assembly.FullName)));

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();

            return services;
        }

        public static IServiceCollection AddControllersWithJsonConfiguration(this IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IOrderMessagePublisher, OrderMessagePublisher>();

            return services;
        }

        public static IServiceCollection AddPreconfiguredJwtAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters.ValidateAudience = false;
                    options.Authority = Environment.GetEnvironmentVariable("IDENTITY_AUTHORITY");
                    options.RequireHttpsMetadata = false;
                    options.BackchannelHttpHandler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = delegate { return true; }
                    };
                });

            return services.AddAuthorization();
        }

        public static IServiceCollection AddMassTransitEventPublishing(this IServiceCollection services)
        {
            return services.AddMassTransit(options =>
            {
                options.AddBus(_ => Bus.Factory.CreateUsingRabbitMq(bus =>
                {
                    // TODO: MOVE STRINGS TO CONSTANTS
                    bus.Host(new Uri("rabbitmq://messagebus"), host =>
                    {
                        host.Username("guest");
                        host.Password("guest");
                    });
                }));
            });
        }
    }
}
