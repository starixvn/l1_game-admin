using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Core.IdentityServer;
using Refit;

namespace GamesAdmin.Api.GameServers
{
    public interface ICrashTurnBaseApiFactory
    {
        ICrashTurnBaseApi SetResultApi(string serviceName);
    }
    public class CrashTurnBaseApiFactory : ICrashTurnBaseApiFactory
    {
        private readonly IAppSettings appSettings;

        public CrashTurnBaseApiFactory(IAppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        public ICrashTurnBaseApi SetResultApi(string serviceName)
        {
            var refitSettings = new RefitSettings
            {
                AuthorizationHeaderValueGetter = IdentityServerManager.GetAccessToken
            };

            var api = string.Format(appSettings.ServiceApiTemplate, serviceName);

            return RestService.For<ICrashTurnBaseApi>(api, refitSettings);
        }
    }
}
