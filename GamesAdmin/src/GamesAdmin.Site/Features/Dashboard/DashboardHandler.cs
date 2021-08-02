using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
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

        private static List<SelectListItem> GenerateGameItems()
            => new List<SelectListItem> {
                new SelectListItem("All", string.Empty),
                new SelectListItem(Game.SeDie.DisplayName, Game.SeDie.Value),
                new SelectListItem(Game.VipSeDie.DisplayName, Game.VipSeDie.Value),
                new SelectListItem(Game.KenoProMax.DisplayName, Game.KenoProMax.Value),
                new SelectListItem(Game.Baccarat.DisplayName, Game.Baccarat.Value),
                new SelectListItem(Game.Roulette.DisplayName, Game.Roulette.Value),
                new SelectListItem(Game.SpecialSeDie.DisplayName, Game.SpecialSeDie.Value),
                new SelectListItem(Game.BlackJack.DisplayName, Game.BlackJack.Value),
                new SelectListItem(Game.VipFishPrawnCrab.DisplayName, Game.VipFishPrawnCrab.Value),
                new SelectListItem(Game.SicBo.DisplayName, Game.SicBo.Value),
                new SelectListItem(Game.VipBaccarat.DisplayName, Game.VipBaccarat.Value),
                new SelectListItem(Game.VipSeDieWar.DisplayName, Game.VipSeDieWar.Value),
                new SelectListItem(Game.FourGuardians.DisplayName, Game.FourGuardians.Value),
                new SelectListItem(Game.Lottery.DisplayName, Game.Lottery.Value),
                new SelectListItem(Game.FishPrawnCrab.DisplayName, Game.FishPrawnCrab.Value),
                new SelectListItem(Game.DragonTiger.DisplayName, Game.DragonTiger.Value),
                new SelectListItem(Game.BigSmall.DisplayName, Game.BigSmall.Value),
                new SelectListItem(Game.BigSmallTurbo.DisplayName, Game.BigSmallTurbo.Value),
                new SelectListItem(Game.OddEvenTurbo.DisplayName, Game.OddEvenTurbo.Value),
                new SelectListItem(Game.MiniSeDie.DisplayName, Game.MiniSeDie.Value),
                new SelectListItem(Game.ThreeCards.DisplayName, Game.ThreeCards.Value),
                new SelectListItem(Game.VietnameseHeroes.DisplayName, Game.VietnameseHeroes.Value),
                };
    }
}