using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CrewCloudCMSApi.Data;
using CrewCloudCMSApi.IdentityModels;
using CrewCloudRepository.Contracts;
using CrewCloudRepository.Models;
using CrewCloudRepository.Repository;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CrewCloudCMSApi
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

            services.AddDbContext<ApplicationDbContext>(options =>
          options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();
            services.AddMvcCore(options =>
            {
               //options.Filters.Add(new AuthorizeFilter());
            }).AddAuthorization();

            services.AddDbContextPool<CrewCloudDBContext>(o => o.UseSqlServer(Configuration.GetConnectionString("CrewCloudDBConnection"), x => x.MigrationsAssembly("CrewCloudRepository")));

            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

            services.AddEntityFrameworkSqlServer();

            services.AddDbContextPool<CrewCloudDBContext>((serviceProvider, optionsBuilder) =>
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("CrewCloudDBConnection"));
                optionsBuilder.UseInternalServiceProvider(serviceProvider);
            });

            services.AddIdentity<AppUser, AppRole>()
             .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAutoMapper(typeof(Startup));

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
           .AddIdentityServerAuthentication(options =>
           {
               options.Authority = Configuration.GetValue<string>("IdentityServerUrl"); ;//IdentityServer URL
               options.RequireHttpsMetadata = false;       //False for local addresses, true ofcourse for live scenarios
               options.ApiName = "CrewCloudCMSApi";
               options.ApiSecret = "CrewCloudApiSecret";


           });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
