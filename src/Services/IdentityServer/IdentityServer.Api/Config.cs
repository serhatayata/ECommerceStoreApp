using IdentityServer4.Models;
using IdentityServer4;
using IdentityModel;

namespace IdentityServer.Api
{
    public class Config
    {
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            #region Basket
            new ApiResource("resource_basket")
            {
                Scopes=
                {
                    "basket_full",
                    "basket_read",
                    "basket_write"
                },
                ApiSecrets =
                {
                    new Secret("basket_secret".Sha256())
                }
            },
	        #endregion
            #region Gateway
            new ApiResource("resource_gateway")
            {
                Scopes=
                {
                    "gateway_full"
                },
                ApiSecrets =
                {
                    new Secret("gateway_secret".Sha256())
                }
            },
	        #endregion
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };

        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                       new IdentityResources.Email(),
                       new IdentityResources.OpenId(),
                       new IdentityResources.Profile(),
                       new IdentityResource()
                       {
                           Name="Roles",
                           DisplayName="Roles",
                           Description="User Roles",
                           UserClaims=new []{ "role"}
                       }
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                #region Basket
                new ApiScope("basket_full","Full permission for Basket API"),
                new ApiScope("basket_read","Read permission for Basket API"),
                new ApiScope("basket_write","Write permission for Basket API"),
	            #endregion
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                #region Javascript Client
                new Client
                {
                    ClientId = "jsClientSPA",
                    ClientName = "Javascript Client",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,

                    // where to redirect to after login
                    RedirectUris = { "https://notificationservice-api:5003/callback.html" },
                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://notificationservice-api:5003/index.html"  },
                    AllowedCorsOrigins =     { "https://notificationservice-api:5003" },
                    AllowOfflineAccess = true,

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "Roles",
                        JwtClaimTypes.Role
                    }
                },
	            #endregion
                #region MVC Client
                new Client
                {
                    ClientId = "mvcClient",
                    ClientName = "MVC Client",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    // where to redirect to after login
                    RedirectUris = { "https://localizationservice-api:5002/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localizationservice-api:5002/signout-callback-oidc" },
                    AllowOfflineAccess = true,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "Roles",
                        JwtClaimTypes.Role
                    }
                },
	            #endregion
                #region ApiClients
                new Client
                {
                    ClientName="Basket Full Client",
                    ClientId="basket_full_client",
                    ClientSecrets= {new Secret("basket_full_secret".Sha256())},
                    AllowedGrantTypes= GrantTypes.ClientCredentials,
                    AllowedScopes={ "basket_full", "basket_read", "basket_write" }
                },
                new Client
                {
                    ClientName="Gateway Full Client",
                    ClientId="gateway_full_client",
                    ClientSecrets= {new Secret("gateway_full_secret".Sha256())},
                    AllowedGrantTypes= GrantTypes.ClientCredentials,
                    AllowedScopes={ "gateway_full" }
                }
	            #endregion
            };
    }
}
