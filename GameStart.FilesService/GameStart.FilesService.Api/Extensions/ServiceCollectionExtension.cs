using GameStart.FilesService.Common.Services;
using GameStart.Shared.Extensions;
using GameStart.Shared.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace GameStart.FilesService.Api.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddSingleton<IFileStorageDirectoryProvider, FileStorageDirectoryProvider>();
            services.AddSingleton<IFileStorage, FileStorage>();
            services.AddSingleton<IFileService, FileService>();

            return services;
        }

        public static IServiceCollection AddAuthenticationWithJwtBearer(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddPreconfiguredJwtBearer();

            return services;
        }

        public static IServiceCollection AddControllersWithFilters(this IServiceCollection services)
        {
            services.AddControllers(config => config.Filters.Add<EmailVerifiedActionFilter>());

            return services;
        }
    }
}
