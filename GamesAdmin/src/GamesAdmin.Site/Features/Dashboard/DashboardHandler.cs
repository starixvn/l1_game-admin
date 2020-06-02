using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Dashboard.Requests;
using GamesAdmin.Site.Features.Dashboard.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Dashboard
{
    public class DashboardHandler : IRequestHandler<GetOnlineUsersRequest, OnlineUsersViewModel>
    {
        private readonly IGameServerService gameServerService;
        private readonly IMapper mapper;

        public DashboardHandler(IGameServerService gameServerService, IMapper mapper)
        {
            this.gameServerService = gameServerService;
            this.mapper = mapper;
        }

        public async Task<OnlineUsersViewModel> Handle(GetOnlineUsersRequest request, CancellationToken cancellationToken)
        {
            var result = await gameServerService.GetOnlineUsers(request.Game);

            return new OnlineUsersViewModel
            {
                TotalReal = result.TotalReal,
                RealUsers = result.RealUsers?.Select(x => mapper.Map<OnlineUserViewModel>(x)),
                TotalUUS = result.TotalUUS,
                UusUsers = result.UusUsers?.Select(x => mapper.Map<OnlineUserViewModel>(x)),
                TotalBots = result.TotalBots,
                Bots = result.Bots?.Select(x => mapper.Map<BotUserViewModel>(x)),
                GameTypeItems = GenerateGameItems()
            };
        }

        private List<SelectListItem> GenerateGameItems()
        => new List<SelectListItem> {
                new SelectListItem("All", string.Empty),
                new SelectListItem(GameType.SeDie.DisplayName, GameType.SeDie.Value),
                new SelectListItem(GameType.KenoProMax.DisplayName, GameType.KenoProMax.Value),
                new SelectListItem(GameType.Baccarat.DisplayName, GameType.Baccarat.Value)
            };
    }
}