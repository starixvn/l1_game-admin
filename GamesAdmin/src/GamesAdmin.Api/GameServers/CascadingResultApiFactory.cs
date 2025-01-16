using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api._Shared.Helpers;
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
            var refitSettings = SettingHelper.GetRefitSettings();

            var api = string.Format(appSettings.ServiceApiTemplate, serviceName);

            return RestService.For<ICascadingSlotResultApi>(api, refitSettings);
        }
      
    }
}