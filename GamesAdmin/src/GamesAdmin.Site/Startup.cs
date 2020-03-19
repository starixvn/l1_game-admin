using System.Reflection;
using AutoMapper;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site.Features.Authentication;
using GamesAdmin.Site.Features.GameSettings;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Refit;

namespace GamesAdmin.Site
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
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(typeof(Startup));

            var appSettings = new AppSettings(Configuration);
            services.AddSingleton<IAppSettings>(appSettings);

            services.AddSingleton(RestService.For<IGameApi>(appSettings.ApiHost));
            services.AddScoped<IGameService, GameService>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddAuthentication("CookieAuthentication")
                  .AddCookie("CookieAuthentication", config =>
                  {
                      config.Cookie.Name = "UserLoginCookie";
                      config.LoginPath = "/Authentication/UserLogin";
                  });

            services.AddControllersWithViews()
                .AddFeatureFolders()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
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
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseCookiePolicy();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=GameConfigs}/{action=Index}/");
            });
        }
    }
}
