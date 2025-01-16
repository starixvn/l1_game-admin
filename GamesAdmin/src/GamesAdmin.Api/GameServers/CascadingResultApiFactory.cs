using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Core.IdentityServer;
using Refit;

namespace GamesAdmin.Api.GameServers
{
    public interface ICascadingResultApiFactory
    {
        ICascadingSlotResultApi SetResultApi(string serviceName);
    }

    public class CascadingResultApiFactory : ICascadingResultApiFactory
    {
        private readonly IAppSettings appSettings;

        public CascadingResultApiFactory(IAppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        public ICascadingSlotResultApi SetResultApi(string serviceName)
        {
            var refitSettings = new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(),
                AuthorizationHeaderValueGetter = async (request, cancellationToken) => await IdentityServerManager.GetAccessToken()
            };

            var api = string.Format(appSettings.ServiceApiTemplate, serviceName);

            return RestService.For<ICascadingSlotResultApi>(api, refitSettings);
        }
      
    }
}