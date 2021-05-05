using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ClientMVC.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace ClientMVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews();
            services.AddRazorPages();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            
            }).AddCookie()
             .AddOpenIdConnect(options =>
              {

                 options.SignInScheme = "Cookies";
                 options.Authority = Configuration.GetValue<string>("IdentityServerUrl");
                 options.ClientId = "MVC";
                 options.ClientSecret = "MVCSecret";
                 options.ResponseType ="code id_token";
                 options.SaveTokens = true;
                 options.GetClaimsFromUserInfoEndpoint = true;
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidAudience = "MVC"
                  };
                  options.RequireHttpsMetadata = false;
                    //options.Scope.Add("openid");
                    //options.Scope.Add("profile");
                  //options.Scope.Add("ResourceApi");
                  //options.Scope.Add("ResourceCMSApi");

                  options.Scope.Add("offline_access");
                  options.ClaimActions.MapJsonKey("role", "role", "role");
                  options.Events = new OpenIdConnectEvents
                  {
                      
                    OnUserInformationReceived = context =>
                      {            
                        return Task.FromResult(0);
                      },
                      OnRedirectToIdentityProvider = n => //for later experiments
                      {

                         // n.ProtocolMessage.SetParameter("external_provider", "Facebook");
                          return Task.FromResult(0);
                      }
                };
              });


            services
                .AddMvc(options =>
                 {
                      options.InputFormatters.Insert(0, new RawJsonBodyInputFormatter());
                     });
                 }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
           
            app.UseCookiePolicy();

            app.UseRouting();
           
            app.UseAuthentication();
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
