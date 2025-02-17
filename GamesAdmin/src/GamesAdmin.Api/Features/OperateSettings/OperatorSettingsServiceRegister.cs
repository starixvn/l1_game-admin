using GamesAdmin.Api.GameServers;
using Microsoft.Extensions.DependencyInjection;

namespace GamesAdmin.Api.Features.OperateSettings
{
    public static class OperatorSettingsServiceRegister
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IOperateSettingsService, OperateSettingsService>();
        }
    }
}
