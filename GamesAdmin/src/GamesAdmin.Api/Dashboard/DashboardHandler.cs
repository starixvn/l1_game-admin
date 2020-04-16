using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.Dashboard.Requests;
using MediatR;

namespace GamesAdmin.Api.Dashboard
{
    public class DashboardHandler : IRequestHandler<GetTodayTotalBetRequest, long>
    {
        private readonly IDashboardService dashboardService;

        public DashboardHandler(IDashboardService dashboardService)
        {
            this.dashboardService = dashboardService;
        }

        public async Task<long> Handle(GetTodayTotalBetRequest request, CancellationToken cancellationToken)
        {
            return await dashboardService.GetTodayTotalBets();
        }
    }
}