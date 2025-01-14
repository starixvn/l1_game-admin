using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Core.IdentityServer;
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
            var refitSettings = new RefitSettings
            {
                AuthorizationHeaderValueGetter = async (request, cancellationToken) => await IdentityServerManager.GetAccessToken()
            };

            var api = string.Format(appSettings.ServiceApiTemplate, serviceName);

            return RestService.For<ICrashGameApi>(api, refitSettings);
        }
    }
}
