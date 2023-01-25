using Serilog;

namespace GameStart.IdentityService.Extensions
{
    public static class HostBuilderExtension
    {
        public static IHostBuilder UsePreconfiguredSerilog(
            this IHostBuilder builder, IConfiguration configuration)
        {
            Serilog.ILogger logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            return builder.UseSerilog(logger);
        }
    }
}
