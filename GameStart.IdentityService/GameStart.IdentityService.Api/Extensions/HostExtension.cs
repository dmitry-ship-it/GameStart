using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;

namespace GameStart.IdentityService.Api.Extensions
{
    public static class HostExtension
    {
        public static IHost UpdateIdentityDbTables(this IHost host, IConfiguration configuration)
        {
            using var scope = host.Services.CreateScope();
            scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

            using var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            context.Database.Migrate();

            var config = new Config(configuration);

            if (!context.Clients.Any())
            {
                foreach (var client in config.Clients)
                {
                    context.Clients.Add(client.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in config.IdentityResources)
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var apiScope in config.GetApiScopes())
                {
                    context.ApiScopes.Add(apiScope.ToEntity());
                }

                context.SaveChanges();
            }

            // FIXME: Add ApiResources later.

            return host;
        }
    }
}
