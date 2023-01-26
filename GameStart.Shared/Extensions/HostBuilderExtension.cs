using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace GameStart.Shared.Extensions
{
    public static class HostBuilderExtension
    {
        public static IHostBuilder UsePreconfiguredSerilog(
            this IHostBuilder builder, IConfiguration configuration)
        {
            ILogger logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            return builder.UseSerilog(logger);
        }
    }
}
