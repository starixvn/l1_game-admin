using System.Threading.Tasks;
using GamesAdmin.Database;

namespace GamesAdmin.Api.Dashboard
{
    public interface IDashboardService
    {
        Task<long> GetTodayTotalBets();
    }

    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            this.dashboardRepository = dashboardRepository;
        }

        public async Task<long> GetTodayTotalBets() => await dashboardRepository.GetTodayTotalBets();
    }
}