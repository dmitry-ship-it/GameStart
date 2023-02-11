using GameStart.Shared;
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
                    ClientId = configuration["ClientId"],
                    ClientName = configuration["ClientName"],
                    ClientSecrets = new[]
                    {
                        new Secret(configuration["ClientSecret"].Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = new[]
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    }.Concat(GetApiScopes().Select(r => r.Name)).ToArray(),
                    RedirectUris = { "https://localhost:7116/" },
                    AllowAccessTokensViaBrowser = true,
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = Constants.IdentityService.TokenLifetime
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

        public IEnumerable<ApiResource> GetApiResources()
        {
            var apiResourcesSection = configuration.GetSection("ApiResources");

            return apiResourcesSection.Get<IEnumerable<string>>().Select(ar => new ApiResource(ar));
        }
    }
}
