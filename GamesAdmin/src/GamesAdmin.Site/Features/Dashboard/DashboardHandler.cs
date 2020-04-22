using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Dashboard.Requests;
using MediatR;

namespace GamesAdmin.Site.Features.Dashboard
{
    public class DashboardHandler : IRequestHandler<GetOnlineUsersRequests, int>,
        IRequestHandler<GetTodayTotalBetsRequest, long>
    {
        private readonly IDashboardService dashboardService;
        private readonly IGameServerService gameServerService;

        public DashboardHandler(IDashboardService dashboardService, IGameServerService gameServerService)
        {
            this.dashboardService = dashboardService;
            this.gameServerService = gameServerService;
        }

        public async Task<int> Handle(GetOnlineUsersRequests request, CancellationToken cancellationToken)
        {
            return await gameServerService.GetOnlineUsers();
        }

        public async Task<long> Handle(GetTodayTotalBetsRequest request, CancellationToken cancellationToken)
        {
            return await dashboardService.GetTodayTotalBets();
        }
    }
}