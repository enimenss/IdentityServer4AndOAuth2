using IdentityServer4.Validation;

using IdentityServer.Entities;
using IdentityServer.ExtensionGrant;
using IdentityServer.Interfaces;
using IdentityServer.Providers;
using IdentityServer.Repositories;
using IdentityServer.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityServer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices<TUser>(this IServiceCollection services) where TUser : IdentityUser<int>, new()
        {
           
            services.AddScoped<IExtensionGrantValidator, ExternalAuthenticationGrant<TUser>>();
            
            services.AddSingleton<HttpClient>();
            return services;
        }
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {

            services.AddScoped<IProviderRepository, ProviderRepository>();
            return services;
        }
        public static IServiceCollection AddProviders<TUser>(this IServiceCollection services) where TUser: IdentityUser<int>,new()
        {
            services.AddTransient<IFacebookAuthProvider, FacebookAuthProvider<TUser>>();
            services.AddTransient<IGoogleAuthProvider, GoogleAuthProvider<TUser>>();
            return services;
        }
    }
}
