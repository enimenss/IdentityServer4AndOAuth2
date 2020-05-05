// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer.Data;
using IdentityServer.ExtensionGrant;
using IdentityServer.Extensions;
using IdentityServer.Interfaces;
using IdentityServer.IdentityModels;
using IdentityServer.Providers;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using CrewCloudRepository.Models;
using CrewCloudRepository.Repository;
using CrewCloudRepository.Contracts;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Threading.Tasks;
using System.Security.Claims;
using IdentityModel;
using IdentityServer.EmailService;

namespace IdentityServer
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Configuration = configuration;
            Environment = environment;
        }


        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // uncomment, if you want to add an MVC-based UI
            //services.AddControllersWithViews();

            var emailConfig = Configuration
         .GetSection("EmailConfiguration")
         .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromMinutes(5);
            });

            services.AddScoped<IEmailSender, EmailSender>();

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //CrewCloudRepositoryDB
            services.AddDbContextPool<CrewCloudDBContext>(o => o.UseSqlServer(Configuration.GetConnectionString("CrewCloudDBConnection"), x => x.MigrationsAssembly("CrewCloudRepository")));

            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

            services.AddEntityFrameworkSqlServer();

            services.AddDbContextPool<CrewCloudDBContext>((serviceProvider, optionsBuilder) =>
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("CrewCloudDBConnection"));
                optionsBuilder.UseInternalServiceProvider(serviceProvider);
            });
            //The end of kaos

            services.AddIdentity<AppUser, AppRole>(options =>
            {          
                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                //options.Lockout.MaxFailedAccessAttempts = 2; //   for   Lock
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
              //  .AddDefaultUI();

            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);

            services.AddAuthentication()
              //.AddCookie("IdentityFacebook")
             .AddFacebook("Facebook",facebookOptions =>
             {
                 facebookOptions.AppId = Configuration.GetValue<string>("FacebookClientId");
                 facebookOptions.AppSecret = Configuration.GetValue<string>("FacebookClientSecret");

                // facebookOptions.SignInScheme = "IdentityFacebook";

                 facebookOptions.ClientId = Configuration.GetValue<string>("FacebookClientId");
                 facebookOptions.ClientSecret = Configuration.GetValue<string>("FacebookClientSecret");

                 facebookOptions.Fields.Add("picture");
                 facebookOptions.Events = new OAuthEvents
                 {
                     OnCreatingTicket = context =>
                     {
                         var identity = (ClaimsIdentity)context.Principal.Identity;
                         //  var profileImg = context.User.GetProperty["picture"]["data"].Value<string>("url");
                         var profileImg = context.User.GetProperty("picture").GetProperty("data").GetProperty("url").ToString();
                         identity.AddClaim(new Claim(JwtClaimTypes.Picture, profileImg));
                         return Task.CompletedTask;
                     },

                 };


             })
             .AddGoogle("Google",googleOptions=>
             {
                 googleOptions.ClientId = "593297442867-bniqrqoddkgkdmtkhjtphou5sui7kv1s.apps.googleusercontent.com";
                 googleOptions.ClientSecret = "97pj3LrCK7HTK8oE-ktZbf8H";

             });

            var config = new Config(Configuration);

            var builder = services.AddIdentityServer(
                          options =>
                          {
                              options.Events.RaiseErrorEvents = true;
                              options.Events.RaiseFailureEvents = true;
                              options.Events.RaiseInformationEvents = true;
                              options.Events.RaiseSuccessEvents = true;
                          }
                )
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(config.Ids)
                .AddInMemoryApiResources(config.Apis)
                .AddInMemoryClients(config.Clients)
                .AddJwtBearerClientAuthentication()
                .AddAspNetIdentity<AppUser>()
                .AddProfileService<ProfileService<AppUser>>()
            //.AddExtensionGrantValidator<LoginAuthenticationGrant<AppUser>>() //feature
            .AddResourceOwnerValidator<PasswordAuthenticationGrant<AppUser>>();



            services.AddServices<AppUser>()
            .AddRepositories()
            .AddProviders<AppUser>();

        


            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();


        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // uncomment if you want to add MVC
            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();

            // uncomment, if you want to add MVC
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

        }
    }

}
