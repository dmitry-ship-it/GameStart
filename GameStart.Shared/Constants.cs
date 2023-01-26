namespace GameStart.Shared
{
    public static class Constants
    {
        public struct IdentityService
        {
            public struct Endpoints
            {
                public const string LoginEndpointName = "login";
                public const string RegisterEndpointName = "register";
                public const string ChallengeEndpointName = "challenge";
                public const string CallbackEndpointName = "callback";
                public const string LogoutEndpointName = "logout";
            }

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
    }
}
