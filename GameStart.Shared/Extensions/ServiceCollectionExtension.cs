using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace GameStart.Shared.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static AuthenticationBuilder AddPreconfiguredJwtBearer(this AuthenticationBuilder builder) =>
            builder.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters.ValidateAudience = false;
                options.TokenValidationParameters.ValidateIssuer = false;
                options.Authority = Environment.GetEnvironmentVariable("IDENTITY_AUTHORITY");
                options.RequireHttpsMetadata = false;
                options.BackchannelHttpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = delegate { return true; }
                };
            });

        public static IServiceCollection AddAllowingEverythingCors(this IServiceCollection services) =>
            services.AddCors(setup => setup.AddDefaultPolicy(policy =>
            {
                policy.SetIsOriginAllowed(_ => true);
                policy.AllowAnyMethod();
                policy.AllowAnyHeader();
                policy.AllowCredentials();
            }));
    }
}
