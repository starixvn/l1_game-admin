using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api._Shared.Helpers;
using Refit;

namespace GamesAdmin.Api.GameServers
{
  public interface ICascadingClusterSlotApiFactory
  {
    ICascadingClusterSlotResultApi SetResultApi(string serviceName);
  }

  public class CascadingClusterSlotApiFactory : ICascadingClusterSlotApiFactory
  {
    private readonly IAppSettings appSettings;

    public CascadingClusterSlotApiFactory(IAppSettings appSettings)
    {
      this.appSettings = appSettings;
    }

    public ICascadingClusterSlotResultApi SetResultApi(string serviceName)
    {
      var refitSettings = SettingHelper.GetRefitSettings();

      var api = string.Format(appSettings.ServiceApiTemplate, serviceName);

      return RestService.For<ICascadingClusterSlotResultApi>(api, refitSettings);
    }

  }
}