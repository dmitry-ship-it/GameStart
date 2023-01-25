using IdentityServer4;
using IdentityServer4.Models;

namespace GameStart.IdentityService
{
    public class Config
    {
        private readonly IConfigurationSection configuration;

        public Config(IConfiguration configuration)
        {
            this.configuration = configuration.GetSection("Identity");
        }

        public IEnumerable<Client> Clients =>
            new[]
            {
                new Client
                {
                    ClientId = "web",
                    //ClientName = configuration["ClientName"],
                    ClientSecrets = new[]
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = new[]
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    },
                    RedirectUris = { "https://localhost:7153/" },
                    AllowAccessTokensViaBrowser = true
                }
            };

        public IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public IEnumerable<ApiScope> GetApiScopes()
        {
            var scopesSection = configuration.GetSection("ApiScopes");
            return scopesSection.Get<IEnumerable<string>>().Select(s => new ApiScope(s));
        }

        //public IEnumerable<ApiResource> GetApiResources() =>
        //    new[]
        //    {
        //        new ApiResource()
        //        {
        //            Name = configuration["ApiResourceName"],
        //            Scopes = ApiScopesNames,
        //        }
        //    };
    }
}
