using GameStart.Shared;

namespace GameStart.MailingService.Api.Extensions
{
    public static class ConfigurationBuilderExtension
    {
        public static IConfigurationBuilder AddEmailConfigurationFile(
            this IConfigurationBuilder configuration,
            IWebHostEnvironment environment)
        {
            return configuration.SetBasePath(environment.ContentRootPath)
                .AddJsonFile(Constants.MailingService.ConfigurationFiles.EmailSettingsFileName,
                    optional: false, reloadOnChange: false);
        }
    }
}
