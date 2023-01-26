namespace GameStart.Shared
{
    public static class Constants
    {
        public struct IdentityServiceEndpoints
        {
            public const string LoginEndpointName = "login";
            public const string RegisterEndpointName = "register";
            public const string ChallengeEndpointName = "challenge";
            public const string CallbackEndpointName = "callback";
            public const string LogoutEndpointName = "logout";
        }

        public struct IdentityServiceConnectionStringNames
        {
            public const string ConfigurationDb = "ConfigurationDbConnection";
            public const string PersistedGrantsDb = "PersistedGrantsDbConnection";
            public const string AccountsDb = "AccountsDbConnection";
        }
    }
}
