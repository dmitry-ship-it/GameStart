using GameStart.OrderingService.Core.Abstractions;
using GameStart.OrderingService.Core.Entities;
using GameStart.OrderingService.Infrastructure;
using GameStart.OrderingService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GameStart.OrderingService.Api.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDbContextWithRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("OrdersDbConnection");

            services.AddDbContext<OrdersDbContext>(options =>
                options.UseMySQL(connectionString, config =>
                    config.MigrationsAssembly(typeof(OrdersDbContext).Assembly.FullName)));

            services.AddScoped<IRepository<Order>, OrderRepository>();
            services.AddScoped<IRepository<Address>, AddressRepository>();
            services.AddScoped<IRepository<Item>, ItemRepository>();

            return services;
        }
    }
}
