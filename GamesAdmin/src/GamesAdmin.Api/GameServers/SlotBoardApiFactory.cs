using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Core.IdentityServer;
using Refit;

namespace GamesAdmin.Api.GameServers
{
    public interface ISlotBoardApiFactory
    {
        ISlotBoardApi GetBoardApi(string serviceName);
    }

    public class SlotBoardApiFactory : ISlotBoardApiFactory
    {
        private readonly IAppSettings appSettings;

        public SlotBoardApiFactory(IAppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        public ISlotBoardApi GetBoardApi(string serviceName)
        {
            var refitSettings = new RefitSettings
            {
                AuthorizationHeaderValueGetter = async (request, cancellationToken) => await IdentityServerManager.GetAccessToken()
            };

            var api = string.Format(appSettings.ServiceApiTemplate, serviceName);

            return RestService.For<ISlotBoardApi>(api, refitSettings);
        }
    }
}