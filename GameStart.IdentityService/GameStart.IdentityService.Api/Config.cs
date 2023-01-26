using IdentityServer4;
using IdentityServer4.Models;

namespace GameStart.IdentityService.Api
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
                    ClientSecrets = new[]
                    {
                        new Secret(configuration["ClientSecret"].Sha256())
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

        // FIXME: Add ApiResources later.
    }
}
