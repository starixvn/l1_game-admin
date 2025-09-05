using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Games;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site._Shared.Services.GeneralGameSettings;
using GamesAdmin.Site.Extensions;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Chips;
using GamesAdmin.Site.Features.CurrencyGroupGames.Requests;
using GamesAdmin.Site.Features.CurrencyGroupGames.ViewModels;
using GamesAdmin.Site.Features.CurrencyGroups;
using GamesAdmin.Site.Features.GameGroupManagement;
using GamesAdmin.Site.Features.GameSettings;
using GamesAdmin.Site.Features.Partners;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace GamesAdmin.Site.Features.CurrencyGroupGames
{
    public class CurrencyGroupGameHandler
        : IRequestHandler<GetOneRequest, EditViewModel>
        , IRequestHandler<EditRequest, ResultBase>
        , IRequestHandler<IndexRequest, IndexViewModel>
        , IRequestHandler<GetGuaranteedFeatureRequest, GuaranteedFeatureViewModel>
        , IRequestHandler<UpdateGuaranteedFeatureRequest, bool>
    {
        private readonly ICurrencyGroupGameService currencyGroupGameService;
        private readonly ICurrencyGroupService currencyGroupService;
        private readonly IChipService chipService;
        private readonly IMapper mapper;
        private readonly IPartnerService partnerService;
        private readonly IGeneralGameSettingService generalGameSettingService;
        private readonly IGameGroupManagementApi gameGroupManagementApi;
        private readonly IGameSettingService gameSettingService;

        public CurrencyGroupGameHandler(ICurrencyGroupGameService currencyGroupGameService, ICurrencyGroupService currencyGroupService, IChipService chipService,
            IMapper mapper, IPartnerService partnerService, IGeneralGameSettingService generalGameSettingService, IGameGroupManagementApi gameGroupManagementApi,
            IGameSettingService gameSettingService)
        {
            this.currencyGroupGameService = currencyGroupGameService;
            this.currencyGroupService = currencyGroupService;
            this.chipService = chipService;
            this.mapper = mapper;
            this.partnerService = partnerService;
            this.generalGameSettingService = generalGameSettingService;
            this.gameGroupManagementApi = gameGroupManagementApi;
            this.gameSettingService = gameSettingService;
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
            result.MaxBetChoiceSetting = await GenerateMaxBetPerChoices(request.GameName, currencyGroupGame?.MaxBetPerChoices);
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

            var games = await gameSettingService.GetAll();
            var gameConfigs = games.FirstOrDefault(g => g.Info.Name == request.GameName);
            var gameGroup = await gameGroupManagementApi.GetByGameId(gameConfigs.Info.GameId);
            if (gameGroup != null)
            {
                result.OtherGameIdsInGroup = gameGroup.GameIds.Where(x => x != gameConfigs.Info.GameId);
                result.OtherGamesInGroup = games.Where(g => result.OtherGameIdsInGroup.Contains(g.Info.GameId)).
                    Select(g => new SelectListItem(g.Info.DisplayName ?? Game.FromGameId((GameId)g.Info.GameId).DisplayName, g.Info
                    .GameId.ToString()));
            }

            return result;
        }

        private async Task<List<GameSettings.ViewModels.MaxBetChoiceViewModel>> GenerateMaxBetPerChoices(string gameName, Dictionary<string, double> maxBetPerChoices)
        {
            var betChoicesSettings = GamesSettings.GetGamesSettings().GetGameFromName(gameName)?.BetChoices;
            var displayBetChoices = GamesSettings.GetGamesSettings().GetGameFromName(gameName)?.DisplayBetChoices;

            if (betChoicesSettings?.Any() == true)
            {
                return betChoicesSettings.Select(choice => new GameSettings.ViewModels.MaxBetChoiceViewModel(
                choice,
                maxBetPerChoices != null && maxBetPerChoices.ContainsKey(choice) ? maxBetPerChoices[choice] : 0,
                displayBetChoices?[choice] ?? choice.ToTitleCase())).ToList();
            }


            // Check if this is a custom AutoRNGIndex game (not in predefined games)

            if (string.IsNullOrWhiteSpace(gameName))
            {
                // For custom AutoRNGIndex games, return empty list since they don't have predefined bet choices
                return new List<GameSettings.ViewModels.MaxBetChoiceViewModel>();
            }

            var gameConfig = await gameSettingService.GetByName(gameName);

            // If BetChoiceMapper doesn't contain the game, try to build bet choices from odds from database
            if (gameConfig?.Info?.Type == GameType.AutoRNGIndex)
            {
                var gameChoices = GenerateMaxBetChoicesFromDatabaseOdds(gameName, maxBetPerChoices, gameConfig);
                if (gameChoices.Any())
                {
                    return gameChoices;
                }
            }

            var betChoices = BetChoiceMapper.Map(Game.FromGameName(gameName));

            return betChoices?.Select(choice => new GameSettings.ViewModels.MaxBetChoiceViewModel(
                choice.Value,
                maxBetPerChoices != null && maxBetPerChoices.ContainsKey(choice.Value) ? maxBetPerChoices[choice.Value] : 0,
                choice.DisplayName)).ToList();
        }

        private List<GameSettings.ViewModels.MaxBetChoiceViewModel> GenerateMaxBetChoicesFromDatabaseOdds(string gameName, Dictionary<string, double> maxBetPerChoices, GameConfig gameConfig)
        {
            // Use the Choices field from gameConfig instead of Odds and OddsDisplayNames
            var choices = gameConfig?.Choices;

            if (choices?.Any() == true)
            {
                return choices.Select(choice => CreateMaxBetChoiceViewModelFromChoice(choice, maxBetPerChoices)).ToList();
            }

            return new List<GameSettings.ViewModels.MaxBetChoiceViewModel>();
        }

        private GameSettings.ViewModels.MaxBetChoiceViewModel CreateMaxBetChoiceViewModelFromChoice(Choice choice, Dictionary<string, double> maxBetPerChoices)
        {
            var maxBetValue = maxBetPerChoices != null && maxBetPerChoices.ContainsKey(choice.Key) ? maxBetPerChoices[choice.Key] : 0;
            var displayName = !string.IsNullOrWhiteSpace(choice.Name) ? choice.Name : choice.Key.ToTitleCase();

            return new GameSettings.ViewModels.MaxBetChoiceViewModel(choice.Key, maxBetValue, displayName);
        }

        public async Task<GuaranteedFeatureViewModel> Handle(GetGuaranteedFeatureRequest request, CancellationToken cancellationToken)
        {
            var config = await currencyGroupGameService.GetConfig(request.GroupId, request.GameId, CurrencyGroupGameConfigType.GuaranteedFeature.Value);
            if (config != null)
            {
                return JsonConvert.DeserializeObject<GuaranteedFeatureViewModel>(config.Value.ToString());
            }
            var result = new GuaranteedFeatureViewModel();
            return result;
        }

        public Task<bool> Handle(UpdateGuaranteedFeatureRequest request, CancellationToken cancellationToken)
            => currencyGroupGameService.UpdateConfig(request.GroupId, request.GameId, request.ConfigValue, CurrencyGroupGameConfigType.GuaranteedFeature.Value);
    }
}