using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.Dashboard
{
    public interface IDashboardService
    {
        Task<int> GetOnlineUsers();
    }

    public class DashboardService : IDashboardService
    {
        private readonly IDashboardApi dashboardApi;

        public DashboardService(IDashboardApi dashboardApi)
        {
            this.dashboardApi = dashboardApi;
        }

        public async Task<int> GetOnlineUsers() => (await dashboardApi.GetOnlineUsers()).TotalReal;
    }
}