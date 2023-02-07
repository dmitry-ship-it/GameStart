using AutoMapper;
using GameStart.CatalogService.Common;
using GameStart.CatalogService.Common.Mapping;
using GameStart.CatalogService.Data;
using GameStart.CatalogService.Data.EntityConfigurations.ValueConverters;
using GameStart.CatalogService.Data.Repositories;
using GameStart.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace GameStart.CatalogService.Api.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDbContextWithRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            var catalogConnectionString = configuration.GetConnectionString(Constants.CatalogService.ConnectionStringNames.CatalogDb);

            services.AddDbContext<CatalogDbContext>(options =>
                options.UseNpgsql(catalogConnectionString, cfg =>
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

        public static IServiceCollection AddPreconfiguredJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters.ValidateAudience = false;
                    options.Authority = configuration["Auth:Authority"];
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
    }
}
