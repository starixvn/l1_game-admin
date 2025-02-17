using GamesAdmin.Site._Shared.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace GamesAdmin.Site.Features.OperateSettings
{
    public static class OperatorSettingsServiceRegister
    {
        public static void RegisterOperatorSettings(this IServiceCollection services, IAppSettings appSettings, RefitSettings refitSettings)
        {
            services.AddScoped<IOperateSettingsService, OperateSettingsService>();
            services.AddSingleton(RestService.For<IOperateSettingsApi>(appSettings.ApiHost, refitSettings));
        }
    }
}
