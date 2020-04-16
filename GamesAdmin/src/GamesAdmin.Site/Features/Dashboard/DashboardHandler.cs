using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Site.Features.Dashboard.Requests;
using MediatR;

namespace GamesAdmin.Site.Features.Dashboard
{
    public class DashboardHandler : IRequestHandler<GetOnlineUsersRequests, int>,
        IRequestHandler<GetTodayTotalBetsRequest, long>
    {
        private readonly IDashboardService dashboardService;

        public DashboardHandler(IDashboardService dashboardService)
        {
            this.dashboardService = dashboardService;
        }

        public async Task<int> Handle(GetOnlineUsersRequests request, CancellationToken cancellationToken)
        {
            return await dashboardService.GetOnlineUsers();
        }

        public async Task<long> Handle(GetTodayTotalBetsRequest request, CancellationToken cancellationToken)
        {
            return await dashboardService.GetTodayTotalBets();
        }
    }
}