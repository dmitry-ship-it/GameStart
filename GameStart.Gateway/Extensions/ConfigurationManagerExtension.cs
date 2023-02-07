using GameStart.Shared;

namespace GameStart.Gateway.Extensions
{
    public static class ConfigurationManagerExtension
    {
        public static ConfigurationManager AddOcelotConfiguration(
            this ConfigurationManager configuration,
            IWebHostEnvironment environment)
        {
            configuration.SetBasePath(environment.ContentRootPath)
                .AddEnvironmentVariables()
                .AddJsonFile(Constants.Gateway.ConfigurationFiles.GatewayConfigurationFileName,
                    optional: false, reloadOnChange: true);

            return configuration;
        }
    }
}
