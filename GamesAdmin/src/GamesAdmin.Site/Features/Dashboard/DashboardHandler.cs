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
using GamesAdmin.Site.Features.GameSettings;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Dashboard
{
    public class DashboardHandler
        : IRequestHandler<GetOnlineUsersRequest, OnlineUsersViewModel>
        , IRequestHandler<GetOnlineUsersByGameRequest, OnlineUsersViewModel>
    {
        private readonly IGameSettingService gameSettingService;
        private readonly IMapper mapper;

        public DashboardHandler(
            IGameSettingService gameSettingService,
            IMapper mapper)
        {            
            this.gameSettingService = gameSettingService;
            this.mapper = mapper;
        }

        public async Task<OnlineUsersViewModel> Handle(GetOnlineUsersRequest request, CancellationToken cancellationToken)
        {
            var result = await gameSettingService.GetOnlineUsers();

            return ConvertToOnlineUserViewModel(result);
        }

        public async Task<OnlineUsersViewModel> Handle(GetOnlineUsersByGameRequest request, CancellationToken cancellationToken)
        {
            var onlineUsers = await gameSettingService.GetOnlineUsers(request.Game);

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
                new SelectListItem(GameType.VipSeDie.DisplayName, GameType.VipSeDie.Value),
                new SelectListItem(GameType.KenoProMax.DisplayName, GameType.KenoProMax.Value),
                new SelectListItem(GameType.Baccarat.DisplayName, GameType.Baccarat.Value),
                new SelectListItem(GameType.Roulette.DisplayName, GameType.Roulette.Value),
                };
    }
}