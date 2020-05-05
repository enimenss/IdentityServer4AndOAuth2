// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


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

       
        public  IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                 new IdentityResource(
                    name: "custom.profile",
                    displayName: "Custom Profile",
                    claimTypes: new[] { "name", "email", "role","picture" })
            };

        public  IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {

                new ApiResource( "CrewCloudApi","MyApi",new[] { JwtClaimTypes.Email,JwtClaimTypes.Name,JwtClaimTypes.Role } ),
                new ApiResource( "CrewCloudCMSApi","MyCMSApi",new[] { JwtClaimTypes.Email,JwtClaimTypes.Name,JwtClaimTypes.Role } )
            {
            ApiSecrets = {
                new Secret( "CrewCloudApiSecret".Sha256() )
            }
            }
            };

        public  IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "MobileApp",
                    ClientSecrets =
                    {
                        new Secret("MobileAppSecret".Sha256())
                    },
                    //http://docs.identityserver.io/en/latest/topics/grant_types.html
                    AllowedGrantTypes = new[] { GrantType.ResourceOwnerPassword },
                    AllowOfflineAccess = true,
                    AllowedScopes =
                    {
                        "openid",
                        "CrewCloudApi",
                        "custom.profile",
                        IdentityServerConstants.StandardScopes.OfflineAccess
                    },

                },
                new Client
                {
                    ClientId = "ExternalProviders",
                    AllowedGrantTypes = new[] {"external" },
                    ClientSecrets =
                    {
                        new Secret("ExternalProvidersSecret".Sha256())
                    },
                    AllowedScopes =
                    {

                        "CrewCloudApi"
                    },
                    AccessTokenType = AccessTokenType.Jwt,
                    AlwaysIncludeUserClaimsInIdToken = true,
                   // AccessTokenLifetime = 86400,
                    AllowOfflineAccess = true,
                    IdentityTokenLifetime = 86400,
                    AlwaysSendClientClaims = true,
                    Enabled = true,
                },

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
                        "custom.profile",
                        "CrewCloudApi",
                        "CrewCloudCMSApi"
                    },
                    AllowOfflineAccess = true,
                },

                new Client
                {
                    ClientId = "Test",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Code,

                    ClientSecrets =
                    {
                        new Secret("Test".Sha256())
                    },

                    RedirectUris = { Configuration.GetValue<string>("MVCUrl")+"/Home/Login" },
                    PostLogoutRedirectUris = { Configuration.GetValue<string>("MVCUrl")+"/Home/Logout" },
                    AccessTokenLifetime = 86400,
                    AllowedScopes =
                    {
                        "openid",
                        "profile",
                        "custom.profile",
                        "CrewCloudApi",
                        "CrewCloudCMSApi"
                    },
                    AllowOfflineAccess = true,
                },

                  new Client
                {
                    ClientId = "AngularImplicit",
                    ClientName = "Angular Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedCorsOrigins = { "http://localhost:4200" },
                    ClientSecrets =
                    {
                        new Secret("AngularSecret".Sha256())
                    },

                    RedirectUris ={ "http://localhost:4200" },
                    PostLogoutRedirectUris = { "http://localhost:4200" },
                    AccessTokenLifetime = 86400,
                    AllowedScopes =
                    {
                        "openid",
                        "profile",
                        "CrewCloudApi"
                    },
                    AllowAccessTokensViaBrowser = true
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
                        "CrewCloudApi"
                    },
                    AllowAccessTokensViaBrowser = true,
                     RequireClientSecret = false,
                      RequirePkce = true
                }



            };
    }
}