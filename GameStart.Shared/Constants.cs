namespace GameStart.Shared
{
    public static class Constants
    {
        public struct IdentityService
        {
            public struct ConnectionStringNames
            {
                public const string ConfigurationDb = "ConfigurationDbConnection";
                public const string PersistedGrantsDb = "PersistedGrantsDbConnection";
                public const string AccountsDb = "AccountsDbConnection";
            }

            public struct ExceptionMessages
            {
                public const string UserNotFound = "User was not found";
                public const string InvalidCredentials = "User was not found or password is incorrect";
                public const string ExternalAuthenticationError = "External authentication error";
            }
        }

        public struct CatalogService
        {
            public struct ConnectionStringNames
            {
                public const string CatalogDb = "CatalogDbConnection";
            }
        }

        public struct OrderingService
        {
            public struct ConnectionStringNames
            {
                public const string OrdersDb = "OrdersDbConnection";
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
    }
}
