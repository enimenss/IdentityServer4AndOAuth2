
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

                new ApiResource( "ResourceApi","MyApi",new[] { JwtClaimTypes.Email,JwtClaimTypes.Name,JwtClaimTypes.Role } ),
                new ApiResource( "ResourceCMSApi","MyCMSApi",new[] { JwtClaimTypes.Email,JwtClaimTypes.Name,JwtClaimTypes.Role } )
            {
            ApiSecrets = {
                new Secret( "ResourceApiSecret".Sha256() )
            }
            }
            };

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