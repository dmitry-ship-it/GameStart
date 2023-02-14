using AutoMapper;
using GameStart.CatalogService.Common;
using GameStart.CatalogService.Common.Consumers;
using GameStart.CatalogService.Common.Mapping;
using GameStart.CatalogService.Data;
using GameStart.CatalogService.Common.Caching;
using GameStart.CatalogService.Data.EntityConfigurations.ValueConverters;
using GameStart.CatalogService.Data.Repositories;
using GameStart.Shared;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using System;

namespace GameStart.CatalogService.Api.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDbContextWithRepositories(this IServiceCollection services)
        {
            services.AddDbContext<CatalogDbContext>(options =>
                options.UseNpgsql(Constants.CatalogService.ConnectionStrings.CatalogDb, cfg =>
                    cfg.MigrationsAssembly(typeof(CatalogDbContext).Assembly.FullName)));

            services.AddScoped<IRepositoryWrapper, CatalogRepositoryWrapper>();
            services.AddScoped<VideoGameManager>();

            return services;
        }

        public static IServiceCollection AddModelsMapper(this IServiceCollection services)
        {
            return services.AddScoped(provider =>
                new MapperConfiguration(cfg =>
                    cfg.AddProfile(new VideoGameViewModelProfile(
                        provider.GetService<IRepositoryWrapper>()))).CreateMapper());
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

        public static IServiceCollection AddControllersWithJsonConfiguration(this IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            return services;
        }

        public static IServiceCollection AddMassTransitEventConsuming(this IServiceCollection services)
        {
            return services.AddMassTransit(options =>
            {
                options.AddConsumer<OrderSubmittedConsumer>();

                options.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(Constants.MessageBus.RabbitMQRoot, host =>
                    {
                        host.Username(Constants.MessageBus.Username);
                        host.Password(Constants.MessageBus.Password);
                    });

                    configurator.ConfigureEndpoints(context);
                });
            });
        }

        public static IServiceCollection AddRedisCache(this IServiceCollection services)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Environment.GetEnvironmentVariable("REDIS_CACHE_URL");
                options.InstanceName = typeof(Program).Namespace;
            });

            services.AddScoped<IRedisCacheService, RedisCacheService>();

            return services;
        }
    }
}
