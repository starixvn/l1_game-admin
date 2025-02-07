using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api._Shared.Helpers;
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
            var refitSettings = SettingHelper.GetRefitSettings();

            var api = string.Format(appSettings.ServiceApiTemplate, serviceName);

            return RestService.For<ISlotBoardApi>(api, refitSettings);
        }
    }
}