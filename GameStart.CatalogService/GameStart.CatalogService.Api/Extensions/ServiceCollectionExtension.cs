using GameStart.CatalogService.Data;
using GameStart.Shared;
using Microsoft.EntityFrameworkCore;

namespace GameStart.CatalogService.Api.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddDbContextsWithIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            static void ConfigureDbContext(DbContextOptionsBuilder options, string connectionString) =>
                options.UseSqlServer(connectionString, o => o.MigrationsAssembly(typeof(CatalogDbContext).Assembly.FullName));

            var catalogConnectionString = configuration.GetConnectionString(Constants.CatalogService.ConnectionStringNames.CatalogDb);

            services.AddDbContext<CatalogDbContext>(options => ConfigureDbContext(options, catalogConnectionString));
        }
    }
}
