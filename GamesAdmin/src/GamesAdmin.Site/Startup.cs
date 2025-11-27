using AutoMapper;
using Coravel;
using GamesAdmin.Site._Shared;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site._Shared.Middlewares;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.AddMediatR(typeof(Startup));
            services.AddAutoMapper(cfg =>
            {
                cfg.ShouldMapMethod = m => false; // Disable extension method mapping to avoid MaxInteger issue with ObjectId
            }, typeof(Startup));

            var appSettings = new AppSettings(Configuration);
            var systemSettings = new SystemSettings(appSettings.SystemType);
            services.AddSingleton<IAppSettings>(appSettings);
            services.AddSingleton<ISystemSettings>(systemSettings);
            services.Configure<ResourceSiteConfiguration>(Configuration.GetSection("ResourceSite"));

            services.AddServices(appSettings);
            services.AddAuthentication(Configuration);
            services.AddHealthChecks();

            AuditConfiguration.Init(Configuration);

            services.AddMvc(o => o.Conventions.Add(new FeatureConvention()))
                  .AddRazorOptions(options =>
                  {
                      // {0} - Action Name
                      // {1} - Controller Name
                      // {2} - Area Name
                      // {3} - Feature Name
                      // Replace normal view location entirely
                      options.ViewLocationFormats.Clear();
                      options.ViewLocationFormats.Add("/Features/{3}/{1}/{0}.cshtml");
                      options.ViewLocationFormats.Add("/Features/{3}/Views/{0}.cshtml");
                      options.ViewLocationFormats.Add("/Features/{3}/{0}.cshtml");
                      options.ViewLocationFormats.Add("/Features/_Shared/{0}.cshtml");
                      options.ViewLocationFormats.Add("/Features/_Shared/Components/{0}.cshtml");
                      options.ViewLocationExpanders.Add(new FeatureViewLocationExpander());
                  });

            services.AddScheduler();
            services.AddScheduleTasks();
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
            app.UseAuthorization();

            app.Use(async (context, next) => {
                context.Request.EnableBuffering();
                await next();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Dashboard}/{action=Index}");
            });
        }
    }
}