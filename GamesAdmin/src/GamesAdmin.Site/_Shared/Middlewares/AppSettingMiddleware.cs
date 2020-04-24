using GamesAdmin.Site._Shared.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GamesAdmin.Site._Shared.Middlewares
{
    public static class AppSettingMiddleware
    {
        public static void AddSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var controlPanelSettings = new GameControlPanelSettings(
                configuration["GameControlPanel:Host"]);
            services.AddSingleton<IGameControlPanelSettings>(controlPanelSettings);
        }
    }
}
