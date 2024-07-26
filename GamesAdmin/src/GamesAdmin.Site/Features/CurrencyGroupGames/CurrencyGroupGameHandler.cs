using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Games;
using GamesAdmin.Site._Shared.Services.GeneralGameSettings;
using GamesAdmin.Site.Extensions;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Chips;
using GamesAdmin.Site.Features.CurrencyGroupGames.Requests;
using GamesAdmin.Site.Features.CurrencyGroupGames.ViewModels;
using GamesAdmin.Site.Features.CurrencyGroups;
using GamesAdmin.Site.Features.Partners;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.CurrencyGroupGames
{
    public class CurrencyGroupGameHandler
        : IRequestHandler<GetOneRequest, EditViewModel>
        , IRequestHandler<EditRequest, ResultBase>
        , IRequestHandler<IndexRequest, IndexViewModel>
    {
        private readonly ICurrencyGroupGameService currencyGroupGameService;
        private readonly ICurrencyGroupService currencyGroupService;
        private readonly IChipService chipService;
        private readonly IMapper mapper;
        private readonly IPartnerService partnerService;
        private readonly IGeneralGameSettingService generalGameSettingService;

        public CurrencyGroupGameHandler(ICurrencyGroupGameService currencyGroupGameService, ICurrencyGroupService currencyGroupService, IChipService chipService,
            IMapper mapper, IPartnerService partnerService, IGeneralGameSettingService generalGameSettingService)
        {
            this.currencyGroupGameService = currencyGroupGameService;
            this.currencyGroupService = currencyGroupService;
            this.chipService = chipService;
            this.mapper = mapper;
            this.partnerService = partnerService;
            this.generalGameSettingService = generalGameSettingService;
        }

        public async Task<EditViewModel> Handle(GetOneRequest request, CancellationToken cancellationToken)
        {
            var games = await generalGameSettingService.GetAllGames();
            var gameId = games.FirstOrDefault(game => game.Name == request.GameName).GameId;
            var currencyGroupGame = await currencyGroupGameService.Get(request.GroupId, gameId);
            var result = mapper.Map<EditViewModel>(currencyGroupGame) ?? new EditViewModel();
            var chipConfigs = await chipService.GetAll();

            result.GameId = gameId;
            result.GroupId = request.GroupId;
            result.AllChips = chipConfigs.OrderBy(chip => chip.Value).ToList();
            result.MaxBetChoiceSetting = GenerateMaxBetPerChoices(request.GameName, currencyGroupGame?.MaxBetPerChoices);
            result.CopySettingGame = request.IsCopySetting;

            if (currencyGroupGame?.BetLevels == null)
            {
                result.BetLevels = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            }

            if (currencyGroupGame?.CoinValues == null)
            {
                result.CoinValues = new double[] { 0, 0, 0 };
            }

            return result;
        }

        public Task<ResultBase> Handle(EditRequest request, CancellationToken cancellationToken)
            => currencyGroupGameService.Update(request.CurrencyGroupGame);

        public async Task<IndexViewModel> Handle(IndexRequest request, CancellationToken cancellationToken)
        {
            var result = new IndexViewModel();
            var currencyGroups = (await currencyGroupService.GetByPartner(request.PartnerId, group => group.Enabled)).ToList();
            var partners = await partnerService.GetAll();
            result.AllCurrencyGroupList = new List<SelectListItem>
            {
                new SelectListItem("None", string.Empty)
            };
            foreach (var partner in partners)
            {
                var currencyGroupsByPartner = (await currencyGroupService.GetByPartner(partner.PartnerId, group => group.Enabled)).ToList();
                var currencyGroupList = currencyGroupsByPartner.Select(group => new SelectListItem($"{partner.Name} - {group.Name}", group.Id));
                result.AllCurrencyGroupList.AddRange(currencyGroupList);
            }

            result.PartnerId = request.PartnerId;
            result.PartnerName = request.PartnerName;
            result.GroupSelectList = currencyGroups.Select(group => new SelectListItem(group.Name, group.Id)).ToList();

            return result;
        }

        private static List<GameSettings.ViewModels.MaxBetChoiceViewModel> GenerateMaxBetPerChoices(string gameName, Dictionary<string, double> maxBetPerChoices)
        {
            var betChoicesSettings = GamesSettings.GetGamesSettings().GetGameFromName(gameName)?.BetChoices;
            if (betChoicesSettings?.Any() == true)
            {
                return betChoicesSettings.Select(choice => new GameSettings.ViewModels.MaxBetChoiceViewModel(
                choice,
                maxBetPerChoices != null && maxBetPerChoices.ContainsKey(choice) ? maxBetPerChoices[choice] : 0,
                choice.ToTitleCase())).ToList();
            }
            if (string.IsNullOrWhiteSpace(gameName) || !Game.ContainsGame(gameName))
            {
                return new List<GameSettings.ViewModels.MaxBetChoiceViewModel>();
            }

            var betChoices = BetChoiceMapper.Map(Game.FromGameName(gameName));

            return betChoices?.Select(choice => new GameSettings.ViewModels.MaxBetChoiceViewModel(
                choice.Value,
                maxBetPerChoices != null && maxBetPerChoices.ContainsKey(choice.Value) ? maxBetPerChoices[choice.Value] : 0,
                choice.DisplayName)).ToList();
        }
    }
}