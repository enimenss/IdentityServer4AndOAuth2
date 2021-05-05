
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
                new IdentityResources.Profile()
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

                    ClientSecrets =
                    {
                        new Secret("MVCSecret".Sha256())
                    },

                    RedirectUris = { Configuration.GetValue<string>("MVCUrl")+"/signin-oidc" },
                    PostLogoutRedirectUris = { Configuration.GetValue<string>("MVCUrl")+"/signout-callback-oidc" },
                    AccessTokenLifetime = 86400,
                    AllowedScopes =
                    {
                        "openid",
                        "profile",
                        "resource.full.access",
                        "resourceCMS.full.access"
                        //"ResourceApi",
                        //"ResourceCMSApi"
                    },
                    RequireClientSecret = true,
                    RequirePkce = false,
                    AllowOfflineAccess = true,
                },
                 
                 new Client
                {
                    ClientId = "AngularCode",
                    ClientName = "Angular Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedCorsOrigins = { "http://localhost:4200" },
                    ClientSecrets =
                    {
                        new Secret("AngularSecret".Sha256())
                    },

                    RedirectUris ={ "http://localhost:4200/about","http://localhost:4200/silent-renew.html" },
                    PostLogoutRedirectUris = { "http://localhost:4200/about","http://localhost:4200/silent-renew.html" },
                    AccessTokenLifetime = 86400,
                    AllowedScopes =
                    {
                        "openid",
                        "profile",
                        "ResourceApi"
                    },
                    AllowAccessTokensViaBrowser = true,
                     RequireClientSecret = false,
                      RequirePkce = true
                }

            };
    }
}