using GameStart.CatalogService.Data;
using GameStart.CatalogService.Data.EntityConfigurations.ValueConverters;
using GameStart.CatalogService.Data.Models;
using GameStart.CatalogService.Data.Repositories;
using GameStart.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GameStart.CatalogService.Api.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDbContextsWithRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            var catalogConnectionString = configuration.GetConnectionString(Constants.CatalogService.ConnectionStringNames.CatalogDb);

            services.AddDbContext<CatalogDbContext>(options =>
            {
                options.UseSqlServer(catalogConnectionString, cfg => cfg.MigrationsAssembly(typeof(CatalogDbContext).Assembly.FullName));
            });

            services.AddScoped<IRepository<VideoGame>, VideoGameRepository>();
            services.AddScoped<IRepository<Language>, LanguageRepository>();

            return services;
        }

        public static IServiceCollection AddPreconfiguredAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters.ValidateAudience = false;
                    options.Authority = configuration["Auth:Authority"];
                });

            services.AddAuthorization();

            // override default behavior
            services.ConfigureApplicationCookie(o =>
            {
                o.Events.OnRedirectToLogin = (ctx) =>
                {
                    if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode < 400)
                    {
                        ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    }

                    return Task.CompletedTask;
                };
                o.Events.OnRedirectToAccessDenied = (ctx) =>
                {
                    if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode < 400)
                    {
                        ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
                    }

                    return Task.CompletedTask;
                };
            });

            return services;
        }

        public static IServiceCollection AddControllersWithJsonOptions(this IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    //options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
                });

            return services;
        }
    }
}
