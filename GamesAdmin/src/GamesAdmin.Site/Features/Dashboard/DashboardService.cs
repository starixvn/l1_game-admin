using System.Threading.Tasks;
using GamesAdmin.Site.Features._Shared;

namespace GamesAdmin.Site.Features.Dashboard
{
    public interface IDashboardService
    {
        Task<long> GetTodayTotalBets();
    }

    public class DashboardService : IDashboardService
    {
        private readonly IStatisticApi dashboardApi;        

        public DashboardService(IStatisticApi dashboardApi, IGameServerService gameServerService)
        {
            this.dashboardApi = dashboardApi;
        }       

        public Task<long> GetTodayTotalBets() => dashboardApi.GetTodayTotalBets();
    }
}