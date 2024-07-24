using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Enumeration.BetChoices;
using GamesAdmin.Core.Games;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site._Shared.Services.GeneralGameSettings;
using GamesAdmin.Site.Extensions;
using GamesAdmin.Site.Features.CurrencyGroupGames;
using GamesAdmin.Site.Features.CurrencyGroups;
using GamesAdmin.Site.Features.GameSettings.Requests;
using GamesAdmin.Site.Features.GameSettings.ViewModels;
using GamesAdmin.Site.Features.GameUsers;
using GamesAdmin.Site.Features.Partners;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.GameSettings
{
    public class GamesHandler
        : IRequestHandler<EditRequest, EditViewModel>
        , IRequestHandler<UpdateRequest, bool>
        , IRequestHandler<CreateRequest, bool>
        , IRequestHandler<IsExistRequest, bool>
        , IRequestHandler<UpdateBotRatioRequest, bool>
        , IRequestHandler<GetStatusRequest, StatusViewModel>
        , IRequestHandler<UpdateMultipleGameStatusRequest, bool>
        , IRequestHandler<UpdateDisabledMessageRequest, bool>
        , IRequestHandler<ReloadGameClientRequest, bool>
        , IRequestHandler<GetJWTRequest, GameServerToken>
        , IRequestHandler<GetMonitorRequest, MonitorViewModel>
        , IRequestHandler<UnderMaintenanceRequest, bool>
        , IRequestHandler<UpdateBetLimitRequest, bool>
        , IRequestHandler<UpdateOddsRequest, string>
        , IRequestHandler<UpdateMarginSettingsRequest, string>
        , IRequestHandler<UpdateJackpotRequest, bool>
        , IRequestHandler<UpdateSymbolWeightByModeRequest, bool>
        , IRequestHandler<GetDisabledMessageRequest, Dictionary<string, string>>
        , IRequestHandler<GetOddsByGameRequest, IEnumerable<OddsViewModel>>
        , IRequestHandler<GetGamesIdByTypeRequest, IEnumerable<Game>>
        , IRequestHandler<GetGamesByTypeRequest, IEnumerable<SelectListItem>>
        , IRequestHandler<CloneSlotGameRequest, string>
        , IRequestHandler<CreateNewGameRequest, CreateNewGameViewModel>
        , IRequestHandler<GetSlotRtpRequest, RtpResult>
        , IRequestHandler<CheckServiceStatusRequest, StatusGameResult>
    {
        private const int MaxLengthOfMessage = 1000;

        private readonly IMapper mapper;

        private readonly IGameSettingService gameService;
        private readonly IGameUserService accountService;
        private readonly IPartnerService partnerService;
        private readonly ICurrencyGroupService currencyGroupService;
        private readonly ICurrencyGroupGameService currencyGroupGameService;
        private readonly ISystemSettings systemSettings;
        private readonly IGeneralGameSettingService generalGameSettingService;
        private readonly GamesSettings gamesSettings;

        public GamesHandler(
            IMapper mapper,
            IGameSettingService gameService,
            IGameUserService accountService,
            IPartnerService partnerService,
            ICurrencyGroupService currencyGroupService,
            ICurrencyGroupGameService currencyGroupGameService,
            ISystemSettings systemSettings,
            IGeneralGameSettingService generalGameSettingService)
        {
            this.mapper = mapper;
            this.gameService = gameService;
            this.accountService = accountService;
            this.partnerService = partnerService;
            this.currencyGroupService = currencyGroupService;
            this.currencyGroupGameService = currencyGroupGameService;
            this.systemSettings = systemSettings;
            this.generalGameSettingService = generalGameSettingService;
            this.gamesSettings = GamesSettings.GetGamesSettings();
        }

        public async Task<IEnumerable<SelectListItem>> Handle(GetGamesByTypeRequest request, CancellationToken cancellationToken)
        {
            var allSlotGames = (await generalGameSettingService.GetAllGames()).Where(game => game.Type == request.Type);
            var types = allSlotGames.Select(game => new SelectListItem($"{game.DisplayName}{""}", game.GameId.ToString()));
            return types;
        }

        public async Task<CreateNewGameViewModel> Handle(CreateNewGameRequest request, CancellationToken cancellationToken)
        {
            var existingGames = (await gameService.GetAll()).Select(game => game.Info.Name);
            var names = Game.GetAllGamesBySystem(systemSettings.System)
                .Select(x => new SelectListItem { Text = x.DisplayName, Value = x.Value })
                .Where(x => !existingGames.Contains(x.Value))
                .ToList();

            var types = Enum.GetValues(typeof(GameType))
                .Cast<GameType>()
                .Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            var difficultyItems = Enum.GetValues(typeof(GameMode))
                .Cast<GameMode>()
                .Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            var addViewModel = new AddViewModel(names, types, difficultyItems);

            var allSlotGames = (await generalGameSettingService.GetAllGames()).Where(game => game.Type == GameType.Slots);
            var addSlotGameViewModel = new AddSlotGameViewModel(allSlotGames, types);

            return new CreateNewGameViewModel(addViewModel, addSlotGameViewModel);
        }

        public async Task<EditViewModel> Handle(EditRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return new EditViewModel();
            }

            var game = await gameService.GetByName(request.Name);

            var viewModel = mapper.Map<EditViewModel>(game);

            if (game != null)
            {
                viewModel.PayoutViewModel = mapper.Map<PayoutViewModel>(game);
                viewModel.BotCount = await accountService.GetBotCount();
                viewModel.BotRatioItems = GenerateBotRatioItems(game.BotSetting.HoursMaxBot, viewModel.BotCount);
                viewModel.DisplayName = Game.ContainsGame(game.Info.Name)
                                        ? Game.FromGameName(game.Info.Name).DisplayName
                                        : game.Info.DisplayName;
                viewModel.CascadingSlotsConfig = game.CascadingSlotsConfig;
                viewModel.SlotRNGConfig = game.SlotRNGConfig;
                viewModel.CrashConfig = game.CrashConfig;
                viewModel.OddsMultipliers = game.OddsMultipliers;

                switch (game.Info.Type)
                {
                    case GameType.Slots:
                        viewModel.SlotViewModel = mapper.Map<SlotViewModel>(game.SlotsConfig);
                        viewModel.PayoutViewModel.IconOdds = GenerateIconOdds(game.SlotsConfig.Icons);
                        viewModel.JackpotViewModel = mapper.Map<JackpotViewModel>(game.SlotsConfig.Jackpot);
                        viewModel.SystemType = systemSettings.System;
                        break;

                    case GameType.CascadingSlots:
                        viewModel.SlotViewModel = mapper.Map<SlotViewModel>(game.CascadingSlotsConfig);
                        break;

                    case GameType.SlotRNG:
                        viewModel.SlotViewModel = new SlotViewModel();
                        viewModel.Type = GameType.SlotRNG;
                        viewModel.PayoutViewModel.BetChoiceBetSettings = GenerateBetSettings(GetBetChoices(game.Info.Name), game.BetLimit);
                        viewModel.PayoutViewModel.BetChoiceOdds = GenerateOdds(game.Info.Name, game.Odds);
                        break;

                    case GameType.Crash:
                        viewModel.SlotViewModel = new SlotViewModel();
                        viewModel.Type = GameType.Crash;
                        break;

                    case GameType.DynamicOdds:
                        viewModel.Type = GameType.DynamicOdds;
                        viewModel.PayoutViewModel.BetChoiceBetSettings = GenerateBetSettings(GetBetChoices(game.Info.Name), game.BetLimit);
                        viewModel.PayoutViewModel.MarginSettings = GenerateMarginSettings(game.Info.Name, game.Margin);
                        break;

                    default:
                        viewModel.Type = GameType.AutoRNG;
                        var choices = GetBetChoices(game.Info.Name);
                        viewModel.PayoutViewModel.BetChoiceBetSettings = GenerateBetSettings(choices, game.BetLimit);
                        viewModel.PayoutViewModel.BetChoiceOdds = GenerateOdds(game.Info.Name, game.Odds);
                        break;
                }
                viewModel.DetailsView = GetDetailsView(viewModel.Type);
            }

            return viewModel;
        }

        public async Task<IEnumerable<OddsViewModel>> Handle(GetOddsByGameRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.GameName))
            {
                return Enumerable.Empty<OddsViewModel>();
            }

            var game = await gameService.GetByName(request.GameName);

            return GenerateOdds(game.Info.Name, game.Odds);
        }

        public async Task<IEnumerable<Game>> Handle(GetGamesIdByTypeRequest request, CancellationToken cancellationToken)
        {
            return (await gameService.GetAll())
                .Where(game => (game.Info.Type == request.GameType || game.Info.Type == 0) && !game.Info.Name.Equals(Game.LuckyWheelValue))
                .Select(game => Game.FromGameName(game.Info.Name))
                .OrderBy(game => game.GameId);
        }

        private string GetDetailsView(GameType type)
        {
            var name = "GameDetails_";
            switch (type)
            {
                case GameType.AutoRNG:
                case GameType.SlotRNG:
                case GameType.Crash:
                case GameType.DynamicOdds:
                    name += "RNG";
                    break;

                default:
                    name += Enum.GetName(typeof(GameType), type);
                    break;
            }
            return name;
        }

        private static List<BotRatioItemViewModel> GenerateBotRatioItems(double[] botRatios, int totalBot)
        {
            var botRatioItems = new List<BotRatioItemViewModel>();

            var hours = Enumerable.Range(00, 24)
                .Select(i => (DateTime.UtcNow.AddHours(i)));

            foreach (var hour in hours)
            {
                botRatioItems.Add(new BotRatioItemViewModel(
                    hour.Hour,
                    hour.ToGreenwichHour(),
                    hour.ToAsiaHour(),
                    botRatios == null || botRatios.Length == 0 ? 0 : botRatios[hour.Hour].ToPercentage(),
                    totalBot));
            }

            return botRatioItems.OrderBy(item => item.GreenwichHour).ToList();
        }

        private List<MaxBetChoiceViewModel> GenerateBetSettings(IEnumerable<Enumeration> betChoices, BetLimit betLimit)
        {
            if (betChoices == default)
            {
                return new List<MaxBetChoiceViewModel>();
            }

            return betChoices.Select(choice => new MaxBetChoiceViewModel(
                choice.Value,
                betLimit.MaxBetChoices?.ContainsKey(choice.Value) == true ? betLimit.MaxBetChoices[choice.Value] : betLimit.MaxBet,
                choice.DisplayName)).ToList();
        }

        private List<OddsViewModel> GenerateOdds(string name, Dictionary<string, double> oddsSettings)
        {
            var items = new List<OddsViewModel>();

            if (string.IsNullOrWhiteSpace(name))
            {
                return items;
            }

            var gameType = Game.FromGameName(name);
            var betOdds = BetOddsMapper.Map(gameType);

            if (betOdds != default)
            {
                foreach (var odd in betOdds)
                {
                    var odds = oddsSettings.ContainsKey(odd.Value) ? oddsSettings[odd.Value] : 0;
                    items.Add(new OddsViewModel(odd.Value, odds, odd.DisplayName));
                }
            }

            var betOdds2 = gamesSettings.GetGameFromName(name)?.Odds;
            if (betOdds2?.Any() == true)
            {
                foreach (var odds in betOdds2)
                {
                    var oddsValue = oddsSettings.ContainsKey(odds) ? oddsSettings[odds] : 0;
                    items.Add(new OddsViewModel(odds, oddsValue, odds.ToTitleCase()));
                }
            }

            return items;
        }

        private static List<IconOddsViewModel> GenerateIconOdds(Dictionary<string, Dictionary<string, double>> iconOdds)
        {
            if (iconOdds == default)
            {
                return new List<IconOddsViewModel>();
            }

            return iconOdds.Select(icon => new IconOddsViewModel(
                icon.Key,
                icon.Value)).ToList();
        }

        private IEnumerable<Enumeration> GetBetChoices(string name)
        {
            var betChoices = gamesSettings.GetGameFromName(name)?.BetChoices;
            if (betChoices?.Any() == true)
            {
                var items = new List<BaseBetChoice>();
                foreach (var betChoice in betChoices)
                {
                    items.Add(new BaseBetChoice(betChoice, betChoice.ToTitleCase()));
                }
                return items;
            }
            if (string.IsNullOrWhiteSpace(name) || !Game.ContainsGame(name))
            {
                return default;
            }

            return BetChoiceMapper.Map(Game.FromGameName(name));
        }

        private List<MarginSettingViewModel> GenerateMarginSettings(string name, IEnumerable<Dictionary<string, double>> marginSettings)
        {
            if (marginSettings?.Any() == true)
            {
                var viewModel = new List<MarginSettingViewModel>();
                foreach (var settings in marginSettings)
                {
                    viewModel.Add(new MarginSettingViewModel(settings));
                }
                return viewModel;
            }
            else
            {
                var Odds = gamesSettings.GetGameFromName(name)?.Odds;
                var defaultSetting = new Dictionary<string, double> { { "max", 0 } };
                if (Odds?.Any() == true)
                {
                    foreach (var odds in Odds)
                    {
                        defaultSetting.Add(odds, 0);
                    }
                }
                return new List<MarginSettingViewModel> { new MarginSettingViewModel(defaultSetting) };
            }
        }

        public Task<bool> Handle(UpdateRequest request, CancellationToken cancellationToken)
        {
            switch (request.GameInfo.Type)
            {
                case GameType.Slots:
                    return gameService.UpdateSlotConfig(new UpdateSlotConfigRequest(request.GameInfo.Name, request.BotSetting.BotEnabled, request.SlotsConfig, request.GameInfo.Type));

                case GameType.CascadingSlots:
                    return gameService
                        .UpdateCascadingSlotsConfig(new UpdateCascadingSlotsConfigRequest(request.GameInfo.Name, request.BotSetting.BotEnabled, request.CascadingSlotsConfig, request.GameInfo.Type));

                case GameType.SlotRNG:
                    return gameService.UpdateSlotRNGConfig(new UpdateSlotRNGConfigRequest(request.GameInfo, request.BotSetting, request.EnableSetting, request.SlotRNGConfig));

                case GameType.Crash:
                    return gameService.UpdateCrashConfig(new UpdateCrashConfigRequest(request.GameInfo, request.BotSetting, request.EnableSetting, request.CrashConfig));

                default:
                    return gameService.UpdateGeneralConfig(new UpdateGeneralConfigRequest(request.GameInfo, request.BotSetting, request.EnableSetting, request.OddsMultipliers));
            }
        }

        public Task<bool> Handle(CreateRequest request, CancellationToken cancellationToken)
        {
            var gameConfig = new GameConfig(
                string.Empty,
                new Dictionary<string, double>(),
                request.GameInfo,
                request.EnableSetting,
                request.BotSettings,
                request.BetLimit,
                request.GameInfo.Type == GameType.Slots ? request.SlotConfigs : null,
                request.GameInfo.Type == GameType.CascadingSlots ? request.CascadingSlotsConfig : null,
                request.GameInfo.Type == GameType.SlotRNG ? request.SlotRNGConfig : null);

            return gameService.Add(gameConfig);
        }

        public Task<bool> Handle(IsExistRequest request, CancellationToken cancellationToken)
        => gameService.IsExistName(request.Name);

        public Task<bool> Handle(UpdateBotRatioRequest request, CancellationToken cancellationToken)
        => gameService.UpdateBotRatios(request.Name, request.BotRatios);

        public async Task<StatusViewModel> Handle(GetStatusRequest request, CancellationToken cancellationToken)
        {
            var gameConfigs = await gameService.GetAll();
            var partners = await partnerService.GetAll();
            var currencyGroups = await currencyGroupService.GetAll();
            var currencyGroupGames = await currencyGroupGameService.GetAll();

            return gameConfigs?.Any() == true
                ? new StatusViewModel(
                    gameConfigs.Select(game => new StatusItemViewModel(game))
                        .OrderBy(game => game.DisplayName)
                        .ToList(),
                    partners.ToList(),
                    currencyGroups.Where(group => group.Enabled).ToList(),
                    currencyGroupGames.ToList())
                : new StatusViewModel(new List<StatusItemViewModel>());
        }

        public async Task<bool> Handle(UpdateMultipleGameStatusRequest request, CancellationToken cancellationToken)
        {
            if (request.Names?.Length == 0)
            {
                return false;
            }

            await gameService.UpdateStatus(request.Names, request.Enabled);

            if (request.Reload)
            {
                await gameService.Reload();
            }

            return true;
        }

        public async Task<bool> Handle(UpdateDisabledMessageRequest request, CancellationToken cancellationToken)
        {
            if (request.Messages[Language.DefaultLanguageValue]?.Length > MaxLengthOfMessage)
            {
                return false;
            }

            return await gameService.UpdateDisabledMessage(request);
        }

        public Task<bool> Handle(ReloadGameClientRequest request, CancellationToken cancellationToken)
        => gameService.Reload();

        public async Task<GameServerToken> Handle(GetJWTRequest request, CancellationToken cancellationToken)
        {
            var result = await gameService.GetJWT(request.Username, request.UserAgent);

            return new GameServerToken
            (
                result.Token,
                request.Game
            );
        }

        public async Task<MonitorViewModel> Handle(GetMonitorRequest request, CancellationToken cancellationToken)
        {
            var gameConfigs = await gameService.GetAll();
            var gameList = new List<MonitorItemViewModel>();

            if (gameConfigs?.Any() == true)
            {
                gameList = gameConfigs
                    .Select(game => mapper.Map<MonitorItemViewModel>(game))
                    .OrderBy(game => game.DisplayName)
                    .ToList();
            }

            return new MonitorViewModel(gameList, await gameService.GetAdminMonitoringSettings(request.Username));
        }

        public Task<bool> Handle(UnderMaintenanceRequest request, CancellationToken cancellationToken)
        => gameService.UnderMaintenance(request);

        public Task<string> Handle(UpdateOddsRequest request, CancellationToken cancellationToken)
        {
            switch (request.Type)
            {
                case GameType.CascadingSlots:
                    return gameService.UpdateCascadingSlotOdds(request.Name, request.CascadingSlotsConfig);

                case GameType.Slots:
                    return gameService.UpdateSlotIconOdds(request.Name, request.ClassicIcons);

                default:
                    return gameService.UpdateOdds(request);
            }
        }

        public Task<string> Handle(UpdateMarginSettingsRequest request, CancellationToken cancellationToken)
        => gameService.UpdateMarginSettings(request);

        public Task<bool> Handle(UpdateBetLimitRequest request, CancellationToken cancellationToken)
        => gameService.UpdateBetLimit(request);

        public Task<bool> Handle(UpdateJackpotRequest request, CancellationToken cancellationToken)
        => gameService.UpdateJackpot(request);

        public Task<Dictionary<string, string>> Handle(GetDisabledMessageRequest request, CancellationToken cancellationToken)
        => gameService.GetDisabledMessage(request.Name);

        public async Task<string> Handle(CloneSlotGameRequest request, CancellationToken cancellationToken)
        => await gameService.CloneNewSlotGame(request.GameId, request.ClonedFrom, request.DisplayName);

        public Task<bool> Handle(UpdateSymbolWeightByModeRequest request, CancellationToken cancellationToken)
        {
            var updateSymbolWeightRequest = new UpdateSymbolWeightRequest(
                    request.Name,
                    new SymbolWeight(request.Mode, request.Weights));
            return gameService.UpdateSymbolWeight(updateSymbolWeightRequest);
        }

        public Task<RtpResult> Handle(GetSlotRtpRequest request, CancellationToken cancellationToken)
        => gameService.GetSlotRtp(request);

        public async Task<StatusGameResult> Handle(CheckServiceStatusRequest request, CancellationToken cancellationToken)
        => await gameService.CheckServiceStatus(request);
    }
}