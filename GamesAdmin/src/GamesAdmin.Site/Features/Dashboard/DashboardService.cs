using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.Dashboard
{
    public interface IDashboardService
    {
        Task<int> GetOnlineUsers();

        Task<long> GetTodayTotalBets();
    }

    public class DashboardService : IDashboardService
    {
        private readonly IDashboardApi dashboardApi;
        private readonly IDashboardGameServerApi gameServerApi;

        public DashboardService(IDashboardApi dashboardApi, IDashboardGameServerApi gameServerApi)
        {
            this.dashboardApi = dashboardApi;
            this.gameServerApi = gameServerApi;
        }

        public async Task<int> GetOnlineUsers() => (await gameServerApi.GetOnlineUsers()).TotalReal;

        public async Task<long> GetTodayTotalBets() => await dashboardApi.GetTodayTotalBets();
    }
}