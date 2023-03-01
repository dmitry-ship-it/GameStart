using Microsoft.Net.Http.Headers;

namespace GameStart.Shared
{
    public static class Constants
    {
        public static readonly string AuthCookieName = HeaderNames.Authorization;

        public struct IdentityService
        {
            public const int TokenLifetime = 36_000;

            public readonly struct ConnectionStrings
            {
                public static readonly string ConfigurationDb =
                    $"Server={Environment.GetEnvironmentVariable("IDENTITY_DB_HOST")};" +
                    $"Database={Environment.GetEnvironmentVariable("IDENTITY_DB_IS4_DATABASE")};" +
                    $"User ID={Environment.GetEnvironmentVariable("IDENTITY_DB_USERNAME")};" +
                    $"Password={Environment.GetEnvironmentVariable("IDENTITY_DB_PASSWORD")}";

                public static readonly string PersistedGrantDb =
                    $"Server={Environment.GetEnvironmentVariable("IDENTITY_DB_HOST")};" +
                    $"Database={Environment.GetEnvironmentVariable("IDENTITY_DB_IS4_DATABASE")};" +
                    $"User ID={Environment.GetEnvironmentVariable("IDENTITY_DB_USERNAME")};" +
                    $"Password={Environment.GetEnvironmentVariable("IDENTITY_DB_PASSWORD")}";

                public static readonly string AccountsDb =
                    $"Server={Environment.GetEnvironmentVariable("IDENTITY_DB_HOST")};" +
                    $"Database={Environment.GetEnvironmentVariable("IDENTITY_DB_ACCOUNTS_DATABASE")};" +
                    $"User ID={Environment.GetEnvironmentVariable("IDENTITY_DB_USERNAME")};" +
                    $"Password={Environment.GetEnvironmentVariable("IDENTITY_DB_PASSWORD")}";
            }

            public struct ExceptionMessages
            {
                public const string UserNotFound = "User was not found";
                public const string InvalidCredentials = "User was not found or password is incorrect";
                public const string ExternalAuthenticationError = "External authentication error";
            }
        }

        public readonly struct MailingService
        {
            public const string ApplicationEmailAddress = "me";

            public readonly struct ConnectionStrings
            {
                public static readonly string HangfireDb =
                    $"mongodb://{Environment.GetEnvironmentVariable("MONGO_HANGFIRE_USERNAME")}" +
                    $":{Environment.GetEnvironmentVariable("MONGO_HANGFIRE_PASSWORD")}" +
                    $"@{Environment.GetEnvironmentVariable("MONGO_HANGFIRE_HOST")}" +
                    $"/{Environment.GetEnvironmentVariable("MONGO_HANGFIRE_DATABASE")}" +
                    "/?authSource=admin";
            }

            public readonly struct ConfigurationFiles
            {
                public const string EmailSettingsFileName = "mailsettings.json";
            }
        }

        public struct CatalogService
        {
            public readonly struct ConnectionStrings
            {
                public static readonly string CatalogDb =
                    $"Host={Environment.GetEnvironmentVariable("CATALOG_DB_HOST")};" +
                    $"Database={Environment.GetEnvironmentVariable("CATALOG_DB_DATABASE")};" +
                    $"Username={Environment.GetEnvironmentVariable("CATALOG_DB_USERNAME")};" +
                    $"Password={Environment.GetEnvironmentVariable("CATALOG_DB_PASSWORD")}";
            }
        }

        public struct OrderingService
        {
            public readonly struct ConnectionStrings
            {
                public static readonly string OrdersDb =
                    $"Server={Environment.GetEnvironmentVariable("ORDERS_DB_HOST")};" +
                    $"Database={Environment.GetEnvironmentVariable("ORDERS_DB_DATABASE")};" +
                    $"User ID={Environment.GetEnvironmentVariable("ORDERS_DB_USERNAME")};" +
                    $"Password={Environment.GetEnvironmentVariable("ORDERS_DB_PASSWORD")}";
            }

            public struct ValidationMessages
            {
                public const string OrderedPhysicalCopyButAddressIsNull = "You must provide your address";
            }
        }

        public struct Gateway
        {
            public struct ConfigurationFiles
            {
                public const string GatewayConfigurationFileName = "ocelot.json";
            }
        }

        public struct MessageBus
        {
            public static readonly Uri RabbitMQRoot = new("rabbitmq://messagebus");
            public const string Username = "guest";
            public const string Password = "guest";
        }
    }
}
