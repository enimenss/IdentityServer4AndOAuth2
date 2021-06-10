using IdentityServer.ExtensionGrant;
using IdentityServer.IdentityModels;
using IdentityServer.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

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

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            services.AddIdentity<AppUser, AppRole>(options =>
            {          
                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                //options.Lockout.MaxFailedAccessAttempts = 2; //   for   Lock
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var builder = services.AddIdentityServer(
                          options =>
                          {
                              options.UserInteraction.LoginUrl = "/authentication/signIn";
                              options.UserInteraction.LogoutUrl = "/authentication/logout";
                          }
                )
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(Config.Ids)
                .AddInMemoryClients(Config.Clients)
                .AddJwtBearerClientAuthentication()
                .AddAspNetIdentity<AppUser>()
                .AddProfileService<ProfileService<AppUser>>();


               // not recommended for production - you need to store your key material somewhere secure
               builder.AddDeveloperSigningCredential();


            services.AddAuthentication();
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);
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
