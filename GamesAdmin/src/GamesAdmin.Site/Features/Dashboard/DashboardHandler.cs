using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Dashboard.Requests;
using GamesAdmin.Site.Features.Dashboard.ViewModels;
using GamesAdmin.Site.Features.GameServers;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Dashboard
{
    public class DashboardHandler
        : IRequestHandler<GetOnlineUsersRequest, OnlineUsersViewModel>
        , IRequestHandler<GetOnlineUsersByGameRequest, OnlineUsersViewModel>
    {
        private readonly IGameServerService gameServerService;
        private readonly IMapper mapper;

        public DashboardHandler(
            IGameServerService gameServerService,
            IShakeThePlateGameServer shakeThePlateGameServer,
            IMapper mapper)
        {
            this.gameServerService = gameServerService;
            this.mapper = mapper;
        }

        public async Task<OnlineUsersViewModel> Handle(GetOnlineUsersRequest request, CancellationToken cancellationToken)
        {
            var result = await gameServerService.GetOnlineUsers(string.Empty);

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

        public async Task<OnlineUsersViewModel> Handle(GetOnlineUsersByGameRequest request, CancellationToken cancellationToken)
        {
            var onlineUsers = await gameServerService.GetOnlineUsers(request.Game);

            return ConvertToOnlineUserViewModel(onlineUsers);
        }

        private OnlineUsersViewModel ConvertToOnlineUserViewModel(OnlineUsers onlineUsers)
        => new OnlineUsersViewModel
        {
            TotalReal = onlineUsers.TotalReal,
            RealUsers = onlineUsers.RealUsers.Select(x => mapper.Map<OnlineUserViewModel>(x)),
            TotalUUS = onlineUsers.TotalUUS,
            UusUsers = onlineUsers.UusUsers.Select(x => mapper.Map<OnlineUserViewModel>(x)),
            TotalBots = onlineUsers.TotalBots,
            Bots = onlineUsers.Bots.Select(x => mapper.Map<BotUserViewModel>(x)),
            GameTypeItems = GenerateGameItems()
        };

        private List<SelectListItem> GenerateGameItems()
            => new List<SelectListItem> {
                new SelectListItem("All", string.Empty),
                new SelectListItem(GameType.SeDie.DisplayName, GameType.SeDie.Value),
                new SelectListItem(GameType.KenoProMax.DisplayName, GameType.KenoProMax.Value),
                new SelectListItem(GameType.Baccarat.DisplayName, GameType.Baccarat.Value)
                };
    }
}