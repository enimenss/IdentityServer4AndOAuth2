
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace IdentityServer
{
    public class Config
    {
        public static IConfiguration Configuration { get; set; }

        public Config(IConfiguration configuration)
        {
            Configuration = configuration;


        }
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),

                new IdentityResource(
                    name: "custom.profile",
                    userClaims: new[] { "sub", "email", "username" },
                    displayName: "Your user identifier")
            };

        public static  IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "ClientMVC",
                    ClientName = "Client MVC",
                    AllowedCorsOrigins = {"https://localhost:44344"},
                    ClientSecrets =
                    {
                        new Secret("MVCSecret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = { "https://localhost:44344/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:44344/signout-callback-oidc" },
                    IdentityTokenLifetime = 1200,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowedScopes =
                    {
                        "openid",
                        "custom.profile"
                    },
                    RequireClientSecret = true,
                    AllowOfflineAccess = true,
                    RequirePkce = false,
                },

                new Client
                {
                    RedirectUris ={"http://localhost:4200"},
                    ClientId = "ClientAngular",
                    ClientName = "Angular Angular",
                    AllowedCorsOrigins = {"http://localhost:4200"},
                    PostLogoutRedirectUris = {"http://localhost:4200"},
                    IdentityTokenLifetime = 1200,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowedScopes =
                    {
                    "openid",
                    "email"
                    },
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    AllowOfflineAccess = true,
                    RequirePkce = true,
                },

                new Client
                {
                ClientId = "ReactNativePKCE",
                ClientName = "React Native Client",
                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris ={"com.clientreactnative:/oauthredirect"},
                PostLogoutRedirectUris = {"com.clientreactnative:/oauthredirect" },
                AccessTokenLifetime = 6000000,
                AbsoluteRefreshTokenLifetime= 120000000,
                IdentityTokenLifetime = 60000000,

                AllowedScopes =
                {
                "openid",
                "profile"
                },
                RequireClientSecret = false,
                RequireConsent = false,
                RequirePkce =true,
                AllowOfflineAccess = true,
                UpdateAccessTokenClaimsOnRefresh = true,
                RefreshTokenUsage = TokenUsage.ReUse
                },

                  new Client
                {
                ClientId = "Xamarin",
                ClientName = "Xamarin Client",
                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris ={"xamarinformsclients://callback"},
                PostLogoutRedirectUris = {"xamarinformsclients://callback" },
                AccessTokenLifetime = 6000000,
                AbsoluteRefreshTokenLifetime= 120000000,
                IdentityTokenLifetime = 60000000,
                BackChannelLogoutSessionRequired = true,
                BackChannelLogoutUri = "xamarinformsclients://callback",

                AllowedScopes =
                {
                "openid",
                "profile"
                },
                RequireClientSecret = false,
                RequireConsent = false,
                RequirePkce =true,
                AllowOfflineAccess = true,
                UpdateAccessTokenClaimsOnRefresh = true,
                RefreshTokenUsage = TokenUsage.ReUse
                }

            };
    }
}