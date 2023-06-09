using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Core.IdentityServer;
using Refit;

namespace GamesAdmin.Api.VoidGame
{
    public interface IVoidRunningTicketApiFactory
    {
        IVoidRunningTicketApi GetVoidRunningTicketApi(string serviceName);
    }

    public class VoidRunningTicketApiFactory : IVoidRunningTicketApiFactory
    {
        private readonly IAppSettings appSettings;

        public VoidRunningTicketApiFactory(IAppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        public IVoidRunningTicketApi GetVoidRunningTicketApi(string serviceName)
        {
            var refitSettings = new RefitSettings
            {
                AuthorizationHeaderValueGetter = IdentityServerManager.GetAccessToken
            };

            var api = string.Format(appSettings.ServiceApiTemplate, serviceName);

            return RestService.For<IVoidRunningTicketApi>(api, refitSettings);
        }
    }
}
