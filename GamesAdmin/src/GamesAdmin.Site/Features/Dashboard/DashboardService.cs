using System;
using System.Threading.Tasks;
using Refit;
using Sentry;
using Sentry.Protocol;

namespace GamesAdmin.Site.Features.Dashboard
{
    public interface IDashboardService
    {
        Task<int> GetOnlineUsers();

        Task<long> GetTodayTotalBets();
    }

    public class DashboardService : IDashboardService
    {
        private readonly IStatisticApi dashboardApi;
        private readonly IStatisticGameServerApi gameServerApi;
        private readonly ISentryClient sentryClient;

        public DashboardService(IStatisticApi dashboardApi, IStatisticGameServerApi gameServerApi)
        {
            this.dashboardApi = dashboardApi;
            this.gameServerApi = gameServerApi;
        }

        public async Task<int> GetOnlineUsers()
        {
            try {
                var onlineUsers = await gameServerApi.GetOnlineUsers();

                return onlineUsers == null ? 0 : onlineUsers.TotalReal;
            }
            catch (Exception ex)
            {
                if (ex is ApiException)
                {
                    var apiEx = ex as ApiException;

                    if (apiEx != null)
                    {
                        sentryClient.CaptureMessage($"Request {apiEx.Uri} error: {apiEx.Message}", SentryLevel.Error);
                    }                    
                }

                sentryClient.CaptureException(ex);
            }

            return 0;
        }     

        public async Task<long> GetTodayTotalBets() => await dashboardApi.GetTodayTotalBets();
    }
}