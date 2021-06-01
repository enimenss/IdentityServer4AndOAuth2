﻿
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


        public IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };

        public  IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {

                new ApiResource
                {
                    Name = "ResourceApi",
                    DisplayName = "Resource Api",
                    Description = "Allow the application to access Resource Api on your behalf",
                    Scopes = new List<string> {"resource.full.access"},
                  //  ApiSecrets = new List<Secret> {new Secret("ResourceApiSecret".Sha256())}
                },
                 new ApiResource
                {
                    Name = "ResourceCMSApi",
                    DisplayName = "Resource CMS Api",
                    Description = "Allow the application to access Resource CMS Api on your behalf",
                    Scopes = new List<string> {"resourceCMS.full.access"},
                   // ApiSecrets = new List<Secret> {new Secret("ResourceCMSApiSecret".Sha256())}
                },

            };

        public IEnumerable<ApiScope> GetApiScopes()
        {
            return new[]
            {
                new ApiScope(name: "resource.full.access", displayName: "Access Resource API Backend"),
                new ApiScope(name: "resourceCMS.full.access", displayName: "Access Resource API Backend"),
             };
        }

        public  IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "MVC",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AllowedCorsOrigins = {Configuration.GetValue<string>("MVCUrl")},

                    ClientSecrets =
                    {
                        new Secret("MVCSecret".Sha256())
                    },

                    RedirectUris = { Configuration.GetValue<string>("MVCUrl")+"/signin-oidc" },
                    PostLogoutRedirectUris = { Configuration.GetValue<string>("MVCUrl")+"/signout-callback-oidc" },
                    AccessTokenLifetime = 86400,
                    AlwaysIncludeUserClaimsInIdToken = true, /*IdentityProvider call in Profile service*/
                    AllowedScopes =
                    {
                        "openid",
                        "profile",
                        "email",
                    },
                    RequireClientSecret = true,
                    RequirePkce = false,
                    AllowOfflineAccess = true,
                    FrontChannelLogoutSessionRequired = true,
                    FrontChannelLogoutUri = Configuration.GetValue<string>("MVCUrl")+"/home/LocalLogout",
                   // BackChannelLogoutUri = Configuration.GetValue<string>("MVCUrl")+"/home/LocalLogout"

                },

                new Client
                {
                ClientId = "AngularPKCE",
                ClientName = "Angular Client",
                AllowedGrantTypes = GrantTypes.Code,
                AllowedCorsOrigins = {"http://localhost:4200","https://diplomski-angular.azurewebsites.net"},

                RedirectUris ={"http://localhost:4200", "https://diplomski-angular.azurewebsites.net"},
                PostLogoutRedirectUris = {"http://localhost:4200", "https://diplomski-angular.azurewebsites.net" },
                AccessTokenLifetime = 60,
                AbsoluteRefreshTokenLifetime= 120000000,
                IdentityTokenLifetime = 60,

                AllowedScopes =
                {
                "openid",
                "profile",
                "resource.full.access",
                "resourceCMS.full.access"
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