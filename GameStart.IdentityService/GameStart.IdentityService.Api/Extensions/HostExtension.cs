using GameStart.IdentityService.Data;
using GameStart.Shared.Extensions;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;

namespace GameStart.IdentityService.Api.Extensions
{
    public static class HostExtension
    {
        public static IHost UseUpdateIdentityDbTables(this IHost host, IConfiguration configuration)
        {
            host.UseAutoCreatingForDatabases(
                typeof(ConfigurationDbContext),
                typeof(PersistedGrantDbContext),
                typeof(AccountsDbContext));

            var scope = host.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

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

            if (!context.ApiResources.Any())
            {
                foreach (var apiResource in config.GetApiResources())
                {
                    context.ApiResources.Add(apiResource.ToEntity());
                }

                context.SaveChanges();
            }

            return host;
        }
    }
}
