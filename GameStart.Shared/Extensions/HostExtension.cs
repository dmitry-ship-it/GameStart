using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GameStart.Shared.Extensions
{
    public static class HostExtension
    {
        public static IHost UseAutoCreatingForDatabases(this IHost host, IServiceProvider services, params Type[] contextTypes)
        {
            using var scope = services.CreateScope();

            foreach (var type in contextTypes)
            {
                using var dbContext = scope.ServiceProvider.GetService(type) as DbContext;

                if (dbContext?.Database?.CanConnect() == true)
                {
                    dbContext.Database.EnsureCreated();
                }
            }

            return host;
        }
    }
}
