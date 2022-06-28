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
using GamesAdmin.Site.Features.Partners;
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
        private readonly IPartnerService partnerService;

        public DashboardHandler(
            IGameSettingService gameSettingService,
            IMapper mapper,
            IPartnerService partnerService)
        {
            this.gameSettingService = gameSettingService;
            this.mapper = mapper;
            this.partnerService = partnerService;
        }

        public async Task<OnlineUsersViewModel> Handle(GetOnlineUsersRequest request, CancellationToken cancellationToken)
        {
            var result = await gameSettingService.GetOnlineUsers();

            return await ConvertToOnlineUserViewModel(result);
        }

        public async Task<OnlineUsersViewModel> Handle(GetOnlineUsersByGameRequest request, CancellationToken cancellationToken)
        {
            var onlineUsers = await gameSettingService.GetOnlineUsers(request.Game);

            return await ConvertToOnlineUserViewModel(onlineUsers);
        }

        private async Task<OnlineUsersViewModel> ConvertToOnlineUserViewModel(OnlineUsers onlineUsers)
        {
            var partners = await GetPartnerSelections();
            var realUsers = mapper.Map<IEnumerable<OnlineUserViewModel>>(onlineUsers.RealUsers)
                .Select(model => AddPartnerField(model, partners))
                .AsEnumerable();
            var testUsers = mapper.Map<IEnumerable<OnlineUserViewModel>>(onlineUsers.TestUsers)
                .Select(model => AddPartnerField(model, partners))
                .AsEnumerable();

            var viewModel = new OnlineUsersViewModel
            {
                TotalRealPlayer = onlineUsers.TotalRealPlayer,
                RealUsers = realUsers,
                TotalTestPlayer = onlineUsers.TotalTestPlayer,
                TestUsers = testUsers,
                TotalBots = onlineUsers.TotalBots,
                Bots = onlineUsers.Bots.Select(x => mapper.Map<BotUserViewModel>(x)),
                GameTypeItems = GenerateGameItems(),
                PartnerItems = partners,
                PartnerIds = partners.Select(partner => partner.Value)
            };

            return viewModel;
        }

        private static OnlineUserViewModel AddPartnerField(OnlineUserViewModel model, IEnumerable<SelectListItem> partners)
        {
            model.PartnerItems = partners;

            return model;
        }

        private async Task<IEnumerable<SelectListItem>> GetPartnerSelections()
        {
            var partners = await partnerService.GetPartnersForSelection(partner => true);

            return partners.Select(partner =>
            {
                partner.Selected = true;

                return partner;
            });
        }

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
                new SelectListItem(Game.KenoMatch1.DisplayName, Game.KenoMatch1.Value),
                new SelectListItem(Game.KenoMatch2.DisplayName, Game.KenoMatch2.Value),
                new SelectListItem(Game.KenoPower1.DisplayName, Game.KenoPower1.Value),
                new SelectListItem(Game.KenoPower2.DisplayName, Game.KenoPower2.Value),
                new SelectListItem(Game.HeroesSlot.DisplayName, Game.HeroesSlot.Value),
                new SelectListItem(Game.KenoMax.DisplayName, Game.KenoMax.Value),
                new SelectListItem(Game.KenoMax2.DisplayName, Game.KenoMax2.Value),
                new SelectListItem(Game.KenoMini.DisplayName, Game.KenoMini.Value),
                new SelectListItem(Game.KenoMini2.DisplayName, Game.KenoMini2.Value),
                new SelectListItem(Game.KenoEast.DisplayName, Game.KenoEast.Value),
                new SelectListItem(Game.KenoWest.DisplayName, Game.KenoWest.Value),
                new SelectListItem(Game.KenoSouth.DisplayName, Game.KenoSouth.Value),
                new SelectListItem(Game.KenoNorth.DisplayName, Game.KenoNorth.Value),
                new SelectListItem(Game.CasinoHoldemPoker.DisplayName, Game.CasinoHoldemPoker.Value),
                new SelectListItem(Game.BitcoinMaster.DisplayName, Game.BitcoinMaster.Value),
                new SelectListItem(Game.HiLo.DisplayName, Game.HiLo.Value),
                new SelectListItem(Game.TexasHoldemPoker.DisplayName, Game.TexasHoldemPoker.Value),
                new SelectListItem(Game.NightClub.DisplayName, Game.NightClub.Value),
                new SelectListItem(Game.TreasureIsland.DisplayName, Game.TreasureIsland.Value),
                new SelectListItem(Game.WayToGoal.DisplayName, Game.WayToGoal.Value),
                new SelectListItem(Game.BlockchainBaccarat.DisplayName, Game.BlockchainBaccarat.Value),
                new SelectListItem(Game.MegaKeno.DisplayName, Game.MegaKeno.Value),
                new SelectListItem(Game.MegaKeno2.DisplayName, Game.MegaKeno2.Value),
                new SelectListItem(Game.MiniSeDie2.DisplayName, Game.MiniSeDie2.Value),
                new SelectListItem(Game.MiniSeDie3.DisplayName, Game.MiniSeDie3.Value),
                new SelectListItem(Game.MiniSeDie4.DisplayName, Game.MiniSeDie4.Value),
                new SelectListItem(Game.UnbalancedBigSmall1.DisplayName, Game.UnbalancedBigSmall1.Value),
                new SelectListItem(Game.UnbalancedBigSmall2.DisplayName, Game.UnbalancedBigSmall2.Value),  
                new SelectListItem(Game.UnbalancedBigSmall3.DisplayName, Game.UnbalancedBigSmall3.Value),  
                new SelectListItem(Game.UnbalancedBigSmall4.DisplayName, Game.UnbalancedBigSmall4.Value),
            };
    }
}