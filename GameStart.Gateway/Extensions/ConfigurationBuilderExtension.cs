using GameStart.Shared;

namespace GameStart.Gateway.Extensions
{
    public static class ConfigurationBuilderExtension
    {
        public static IConfigurationBuilder AddOcelotConfiguration(
            this IConfigurationBuilder configuration,
            IWebHostEnvironment environment)
        {
            return configuration.SetBasePath(environment.ContentRootPath)
                .AddEnvironmentVariables()
                .AddJsonFile(Constants.Gateway.ConfigurationFiles.GatewayConfigurationFileName,
                    optional: false, reloadOnChange: true);
        }
    }
}
