using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Dashboard.Requests;
using GamesAdmin.Site.Features.Dashboard.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Dashboard
{
    public class DashboardHandler : IRequestHandler<GetOnlineUsersRequests, OnlineUsersViewModel>
    {
        private readonly IGameServerService gameServerService;

        public DashboardHandler(IGameServerService gameServerService)
        {
            this.gameServerService = gameServerService;
        }

        public async Task<OnlineUsersViewModel> Handle(GetOnlineUsersRequests request, CancellationToken cancellationToken)
        {
            var result =  await gameServerService.GetOnlineUsers();
            return new OnlineUsersViewModel
            {
                TotalReal = result.TotalReal,
                RealUsers = result.RealUsers?.Select(x => new OnlineUserViewModel
                {
                    Character = x.Character,
                    Currency = x.Currency,
                    Id = x.Id,
                    Name = x.Name
                }),
                TotalUUS = result.TotalUUS,
                UusUsers = result.UusUsers?.Select(x => new OnlineUserViewModel
                {
                    Character = x.Character,
                    Currency = x.Currency,
                    Id = x.Id,
                    Name = x.Name
                }),
            };
        }
    }
}