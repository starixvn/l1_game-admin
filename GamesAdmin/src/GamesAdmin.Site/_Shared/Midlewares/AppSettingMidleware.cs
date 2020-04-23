using GamesAdmin.Site._Shared.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GamesAdmin.Site._Shared.Midlewares
{
    public static class AppSettingMidleware
    {
        public static void AddSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var controlPanelSettings = new GameControlPanelSettings(
                configuration["GameControlPanel:Host"]);
            services.AddSingleton<IGameControlPanelSettings>(controlPanelSettings);
        }
    }
}
