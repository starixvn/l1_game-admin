using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api._Shared.Helpers;
using Refit;

namespace GamesAdmin.Api.GameServers
{
    public interface ICrashGameApiFactory
    {
        ICrashGameApi SetResultApi(string serviceName);
    }
    public class CrashGameApiFactory : ICrashGameApiFactory
    {
        private readonly IAppSettings appSettings;

        public CrashGameApiFactory(IAppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        public ICrashGameApi SetResultApi(string serviceName)
        {
            var refitSettings = SettingHelper.GetRefitSettings();

            var api = string.Format(appSettings.ServiceApiTemplate, serviceName);

            return RestService.For<ICrashGameApi>(api, refitSettings);
        }
    }
}
