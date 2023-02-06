using GameStart.IdentityService.Data;
using GameStart.IdentityService.Data.Models;
using GameStart.Shared;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GameStart.IdentityService.Api.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddDbContextsWithIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            static void ConfigureDbContext(DbContextOptionsBuilder options, string connectionString) =>
                options.UseSqlServer(connectionString, o =>
                    o.MigrationsAssembly(typeof(AccountsDbContext).Assembly.FullName));

            var configurationConnectionString = configuration.GetConnectionString(
                Constants.IdentityService.ConnectionStringNames.ConfigurationDb);

            var persistedGrantsConnectionString = configuration.GetConnectionString(
                Constants.IdentityService.ConnectionStringNames.PersistedGrantsDb);

            var accountsConnectionString = configuration.GetConnectionString(
                Constants.IdentityService.ConnectionStringNames.AccountsDb);

            services.AddDbContext<ConfigurationDbContext>(options =>
                ConfigureDbContext(options, configurationConnectionString));

            services.AddDbContext<PersistedGrantDbContext>(options =>
                ConfigureDbContext(options, persistedGrantsConnectionString));

            services.AddDbContext<AccountsDbContext>(options =>
                ConfigureDbContext(options, accountsConnectionString));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AccountsDbContext>()
                .AddDefaultTokenProviders();

            // email used as username if user signs in with Google for the first time
            services.Configure<IdentityOptions>(options => options.User.RequireUniqueEmail = true);

            // override default AspIdentity's behavior
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
        }

        public static IServiceCollection AddCustomCorsPolicy(
            this IServiceCollection services)
        {
            return services.AddSingleton<ICorsPolicyService>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<DefaultCorsPolicyService>>();

                return new DefaultCorsPolicyService(logger)
                {
                    AllowAll = true,
                };
            });
        }

        public static IServiceCollection AddPreconfiguredIdentityServer(this IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddConfigurationStore<ConfigurationDbContext>()
                .AddOperationalStore<PersistedGrantDbContext>()
                .AddAspNetIdentity<User>()
                .AddDeveloperSigningCredential();

            return services;
        }

        public static IServiceCollection AddGoogleAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Add(JwtClaimTypes.Subject, ClaimTypes.NameIdentifier);

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.SaveTokens = true;
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.ClientId = configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
                });

            return services;
        }
    }
}