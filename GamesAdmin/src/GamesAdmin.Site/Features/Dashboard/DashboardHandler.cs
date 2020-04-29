using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Dashboard.Requests;
using MediatR;

namespace GamesAdmin.Site.Features.Dashboard
{
    public class DashboardHandler : IRequestHandler<GetOnlineUsersRequests, int>
    {
        private readonly IGameServerService gameServerService;

        public DashboardHandler(IGameServerService gameServerService)
        {
            this.gameServerService = gameServerService;
        }

        public async Task<int> Handle(GetOnlineUsersRequests request, CancellationToken cancellationToken)
        {
            return await gameServerService.GetOnlineUsers();
        }
    }
}