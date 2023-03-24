using AutoMapper;
using GameStart.CatalogService.Common;
using GameStart.CatalogService.Common.Caching;
using GameStart.CatalogService.Common.Consumers;
using GameStart.CatalogService.Common.Elasticsearch;
using GameStart.CatalogService.Common.Elasticsearch.Search;
using GameStart.CatalogService.Common.Mapping;
using GameStart.CatalogService.Common.Services;
using GameStart.CatalogService.Data;
using GameStart.CatalogService.Data.EntityConfigurations.ValueConverters;
using GameStart.CatalogService.Data.Models;
using GameStart.CatalogService.Data.Repositories;
using GameStart.Shared;
using GameStart.Shared.Extensions;
using GameStart.Shared.Filters;
using GameStart.Shared.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Nest;
using System.Text.Json.Serialization;

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
            services.AddScoped<ISelectorByPage<VideoGame>, VideoGameRepository>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<VideoGameManager>();
            services.AddSingleton<IClock, Clock>();
            services.AddSingleton<IJsonSafeOptionsProvider, JsonSafeOptionsProvider>();

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
                .AddPreconfiguredJwtBearer();

            return services.AddAuthorization();
        }

        public static IServiceCollection AddControllersWithJsonConfigurationAndFilters(this IServiceCollection services)
        {
            services.AddControllers(config => config.Filters.Add<EmailVerifiedActionFilter>())
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

        public static IServiceCollection AddElasticsearch(this IServiceCollection services)
        {
            var settings = new ConnectionSettings(new Uri(Environment.GetEnvironmentVariable("ELASTICSEARCH_URI")!));
            settings.ThrowExceptions();
            settings.IncludeServerStackTraceOnError();
            settings.EnableApiVersioningHeader();
            settings.BasicAuthentication(
                Environment.GetEnvironmentVariable("ELASTICSEARCH_USERNAME")!,
                Environment.GetEnvironmentVariable("ELASTICSEARCH_PASSWORD")!
            );

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);
            services.AddScoped<IElasticsearchService<VideoGame, VideoGameSearchRequest>, VideoGameSearchService>();

            return services;
        }
    }
}
