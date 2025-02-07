using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api._Shared.Helpers;
using Refit;
using Sentry;

namespace GamesAdmin.Api.VoidGame
{
    public interface IVoidRunningTicketApiFactory
    {
        IVoidRunningTicketApi GetVoidRunningTicketApi(string serviceName);
    }

    public class VoidRunningTicketApiFactory : IVoidRunningTicketApiFactory
    {
        private readonly IAppSettings appSettings;
        private readonly ISentryClient sentryClient;

        public VoidRunningTicketApiFactory(IAppSettings appSettings, ISentryClient sentryClient)
        {
            this.appSettings = appSettings;
            this.sentryClient = sentryClient;
        }

        public IVoidRunningTicketApi GetVoidRunningTicketApi(string serviceName)
        {
            var refitSettings = SettingHelper.GetRefitSettings();

            var api = string.Format(appSettings.ServiceApiTemplate, serviceName);
            this.sentryClient.CaptureMessage($"Void Ticket URL: {api}");
            return RestService.For<IVoidRunningTicketApi>(api, refitSettings);
        }
    }
}
