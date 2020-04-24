using GamesAdmin.Site.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GamesAdmin.Site._Shared.Middlewares
{
    public static class AuthenticationMiddleware
    {
        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataProtection()
                .PersistKeysToMongoDb(configuration)
                .SetApplicationName(configuration["ApplicationName"]);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                  .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, config =>
                  {
                      config.Cookie.Name = "UserLoginCookie";
                      config.LoginPath = "/Authentication/UserLogin";
                  });
        }
    }
}
