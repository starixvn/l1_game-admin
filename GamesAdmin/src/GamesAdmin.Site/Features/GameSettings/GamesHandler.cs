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
using GamesAdmin.Site.Features.GameGroupManagement;
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
        , IRequestHandler<CreateRequest, GameCreationResult>
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
        , IRequestHandler<UpdateSlotBotConfigRequest, bool>
        , IRequestHandler<UpdatePlinkoConfigRequest, string>
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
        private readonly IGameGroupManagementApi gameGroupManagementApi;

        public GamesHandler(
            IMapper mapper,
            IGameSettingService gameService,
            IGameUserService accountService,
            IPartnerService partnerService,
            ICurrencyGroupService currencyGroupService,
            ICurrencyGroupGameService currencyGroupGameService,
            ISystemSettings systemSettings,
            IGeneralGameSettingService generalGameSettingService,
            IGameGroupManagementApi gameGroupManagementApi)
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
            this.gameGroupManagementApi = gameGroupManagementApi;
        }

        public async Task<IEnumerable<SelectListItem>> Handle(GetGamesByTypeRequest request, CancellationToken cancellationToken)
        {
            var allGames = await generalGameSettingService.GetAllGames();
            if (allGames == null)
            {
                return Enumerable.Empty<SelectListItem>();
            }

            var allSlotGames = allGames.Where(game => game.Type == request.Type);
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

            var allGames = await generalGameSettingService.GetAllGames();
            var allSlotGames = allGames?.Where(game => game.Type == GameType.Slots) ?? Enumerable.Empty<GamesAdmin.Core.Models.GeneralGameSetting>();
            var addSlotGameViewModel = new AddSlotGameViewModel(allSlotGames, types);

            return new CreateNewGameViewModel(addViewModel, addSlotGameViewModel);
        }

        public async Task<EditViewModel> Handle(EditRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return new EditViewModel();
            }

            var games = await gameService.GetAll();
            var game = games.FirstOrDefault(g => g.Info.Name == request.Name);

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
                viewModel.TournamentConfig = game.TournamentConfig;
                viewModel.VideoStreamConfig = game.VideoStreamConfig;
                viewModel.PlinkoConfig = game.PlinkoConfig;

                var gameGroup = await gameGroupManagementApi.GetByGameId(game.Info.GameId);
                if (gameGroup != null)
                {
                    viewModel.OtherGameIdsInGroup = gameGroup.GameIds.Where(x => x != game.Info.GameId);
                    viewModel.OtherGamesInGroup = games.Where(g => viewModel.OtherGameIdsInGroup.Contains(g.Info.GameId))
                        .Select(g => new SelectListItem(g.Info.DisplayName ?? Game.FromGameName(g.Info.Name).DisplayName, g.Info.GameId.ToString()));
                }

                ConfigureViewModelByGameType(viewModel, game);
                viewModel.DetailsView = GetDetailsView(viewModel.Type);
            }

            return viewModel;
        }

        private void ConfigureViewModelByGameType(EditViewModel viewModel, GameConfig game)
        {
            switch (game.Info.Type)
            {
                case GameType.Slots:
                    ConfigureSlotsGame(viewModel, game);
                    break;

                case GameType.CascadingSlots:
                    ConfigureCascadingSlotsGame(viewModel, game);
                    break;

                case GameType.SlotRNG:
                    ConfigureSlotRNGGame(viewModel, game);
                    break;

                case GameType.FixedStepsCrashTurnBase:
                case GameType.CrashTurnBase:
                case GameType.Crash:
                    ConfigureCrashGame(viewModel);
                    break;

                case GameType.DynamicOdds:
                    ConfigureDynamicOddsGame(viewModel, game);
                    break;

                case GameType.Marble:
                    ConfigureMarbleGame(viewModel, game);
                    break;

                case GameType.AutoRNGIndex:
                    ConfigureAutoRNGIndexGame(viewModel, game);
                    break;
                case GameType.Plinko:
                    ConfigurePlinkoGame(viewModel, game);
                    break;
                case GameType.MarbleProduct:
                    ConfigureMarbleProductGame(viewModel, game);
                    break;

                default:
                    ConfigureDefaultGame(viewModel, game);
                    break;
            }
        }

        private void ConfigureSlotsGame(EditViewModel viewModel, GameConfig game)
        {
            viewModel.SlotViewModel = mapper.Map<SlotViewModel>(game.SlotsConfig);
            viewModel.PayoutViewModel.IconOdds = GenerateIconOdds(game.SlotsConfig.Icons);
            viewModel.JackpotViewModel = mapper.Map<JackpotViewModel>(game.SlotsConfig.Jackpot);
            viewModel.JackpotViewModel.StakeLevels = game.SlotsConfig.StakeLevels;
            viewModel.SystemType = systemSettings.System;
        }

        private void ConfigureCascadingSlotsGame(EditViewModel viewModel, GameConfig game)
        {
            viewModel.SlotViewModel = mapper.Map<SlotViewModel>(game.CascadingSlotsConfig);

            if (viewModel.CascadingSlotsConfig.OddsByCounts.ContainsKey("9") == false)
            {
                viewModel.CascadingSlotsConfig.OddsByCounts.Add("9", new List<OddsByCount>
                {
                    new OddsByCount(4,4,0),
                    new OddsByCount(5,5,0),
                    new OddsByCount(6,30,0),
                });
            }
        }

        private void ConfigureSlotRNGGame(EditViewModel viewModel, GameConfig game)
        {
            viewModel.SlotViewModel = new SlotViewModel();
            viewModel.Type = GameType.SlotRNG;
            viewModel.PayoutViewModel.BetChoiceBetSettings = GenerateBetSettings(GetBetChoices(game.Info.Name), game.BetLimit);
            viewModel.PayoutViewModel.BetChoiceOdds = GenerateOdds(game.Info.Name, game.Odds);
        }

        private static void ConfigureCrashGame(EditViewModel viewModel)
        {
            viewModel.SlotViewModel = new SlotViewModel();
            viewModel.Type = GameType.Crash;
        }

        private void ConfigureDynamicOddsGame(EditViewModel viewModel, GameConfig game)
        {
            viewModel.Type = GameType.DynamicOdds;
            viewModel.PayoutViewModel.BetChoiceBetSettings = GenerateBetSettings(GetBetChoices(game.Info.Name), game.BetLimit);
            viewModel.PayoutViewModel.MarginSettings = GenerateMarginSettings(game.Info.Name, game.Margin);
        }

        private void ConfigureMarbleGame(EditViewModel viewModel, GameConfig game)
        {
            viewModel.Type = GameType.Marble;
            viewModel.PayoutViewModel.BetChoiceBetSettings = GenerateBetSettings(GetBetChoices(game.Info.Name), game.BetLimit);
            if (game.Info.UseMargin == true)
            {
                viewModel.PayoutViewModel.DisplayMargins = gamesSettings.GetGameFromName(game.Info.Name)?.DisplayOdds;
                viewModel.PayoutViewModel.MarginSettings = GenerateMarginSettings(game.Info.Name, game.Margin);
            }
            else
            {
                viewModel.PayoutViewModel.BetChoiceOdds = GenerateOdds(game.Info.Name, game.Odds);
            }
        }

        private void ConfigureMarbleProductGame(EditViewModel viewModel, GameConfig game)
        {
            viewModel.Type = GameType.MarbleProduct;
            var marbleProductChoices = GetBetChoices(game.Choices);
            viewModel.PayoutViewModel.BetChoiceBetSettings = GenerateBetSettings(marbleProductChoices, game.BetLimit);
            viewModel.PayoutViewModel.DisplayMargins = GetBetChoicesDisplayNames(game.Choices);
            viewModel.PayoutViewModel.MarginSettings = GenerateMarginSettings(marbleProductChoices, game.Margin);

        }

        private void ConfigureAutoRNGIndexGame(EditViewModel viewModel, GameConfig game)
        {
            viewModel.Type = GameType.AutoRNGIndex;
            var autoRngChoices = GetBetChoices(game.Info.Name, game.Odds, game.OddsDisplayNames);
            viewModel.PayoutViewModel.BetChoiceBetSettings = GenerateBetSettings(autoRngChoices, game.BetLimit);
            viewModel.PayoutViewModel.BetChoiceOdds = GenerateOdds(game.Info.Name, game.Odds, game.OddsDisplayNames);
        }

        private void ConfigurePlinkoGame(EditViewModel viewModel, GameConfig game)
        {
            viewModel.Type = GameType.Plinko;
            var autoRngChoices = GetBetChoices(game.Info.Name, game.Odds, game.OddsDisplayNames);
            viewModel.PayoutViewModel.BetChoiceBetSettings = GenerateBetSettings(autoRngChoices, game.BetLimit);
            viewModel.PayoutViewModel.BetChoiceOdds = GenerateOdds(game.Info.Name, game.Odds, game.OddsDisplayNames);
        }

        private void ConfigureDefaultGame(EditViewModel viewModel, GameConfig game)
        {
            viewModel.Type = GameType.AutoRNG;
            var choices = GetBetChoices(game.Info.Name);
            viewModel.PayoutViewModel.BetChoiceBetSettings = GenerateBetSettings(choices, game.BetLimit);
            viewModel.PayoutViewModel.BetChoiceOdds = GenerateOdds(game.Info.Name, game.Odds);
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
                case GameType.Marble:
                case GameType.MarbleProduct:
                case GameType.AutoRNGIndex:
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
                choice.DisplayName.ToTitleCase())).ToList();
        }

        private List<OddsViewModel> GenerateOdds(string name, Dictionary<string, double> oddsSettings, Dictionary<string, string> oddsDisplayNames = null)
        {
            var items = new List<OddsViewModel>();

            if (string.IsNullOrWhiteSpace(name))
            {
                return items;
            }

            // Check if this is a custom AutoRNGIndex game (not in predefined games)
            var isCustomGame = !Game.ContainsGame(name);

            if (isCustomGame)
            {
                // For custom AutoRNGIndex games, use only the odds stored in the database
                if (oddsSettings?.Any() == true)
                {
                    foreach (var odds in oddsSettings)
                    {
                        var displayName = oddsDisplayNames?.ContainsKey(odds.Key) == true
                            ? oddsDisplayNames[odds.Key]
                            : odds.Key.ToTitleCase();
                        items.Add(new OddsViewModel(odds.Key, odds.Value, displayName));
                    }
                }
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
            var displayOdds = gamesSettings.GetGameFromName(name)?.DisplayOdds;
            if (betOdds2?.Any() == true)
            {
                foreach (var odds in betOdds2)
                {
                    var oddsValue = oddsSettings.ContainsKey(odds) ? oddsSettings[odds] : 0;
                    items.Add(new OddsViewModel(odds, oddsValue, displayOdds?[odds] ?? odds.ToTitleCase()));
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

        private static IEnumerable<BaseBetChoice> GetBetChoices(List<Choice> choices)
        {
            var items = new List<BaseBetChoice>();
            foreach (var choice in choices)
            {
                items.Add(new BaseBetChoice(choice.Key, choice.Name));
            }

            return items;
        }

        private static Dictionary<string, string> GetBetChoicesDisplayNames(List<Choice> choices)
        {
            var items = new Dictionary<string, string>();
            foreach (var choice in choices)
            {
                items.Add(choice.Key, choice.Name);
            }
            return items;
        }

        private IEnumerable<Enumeration> GetBetChoices(string name, Dictionary<string, double> oddsSettings = null, Dictionary<string, string> oddsDisplayNames = null)
        {
            var betChoices = gamesSettings.GetGameFromName(name)?.BetChoices;
            var displayBetChoices = gamesSettings.GetGameFromName(name)?.DisplayBetChoices;

            if (betChoices?.Any() == true)
            {
                var items = new List<BaseBetChoice>();
                foreach (var betChoice in betChoices)
                {
                    items.Add(new BaseBetChoice(betChoice, displayBetChoices?[betChoice] ?? betChoice.ToTitleCase()));
                }
                return items;
            }

            // Check if this is a custom AutoRNGIndex game (not in predefined games)
            if (string.IsNullOrWhiteSpace(name) || !Game.ContainsGame(name))
            {
                // For custom AutoRNGIndex games, create bet choices from odds settings
                if (oddsSettings?.Any() == true)
                {
                    var items = new List<BaseBetChoice>();
                    foreach (var odds in oddsSettings)
                    {
                        var displayName = oddsDisplayNames?.ContainsKey(odds.Key) == true
                            ? oddsDisplayNames[odds.Key]
                            : odds.Key.ToTitleCase();
                        items.Add(new BaseBetChoice(odds.Key, displayName));
                    }
                    return items;
                }
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

        private static List<MarginSettingViewModel> GenerateMarginSettings(IEnumerable<Enumeration> choices, IEnumerable<Dictionary<string, double>> marginSettings)
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
                var defaultSetting = new Dictionary<string, double> { { "max", 0 } };
                if (choices?.Any() == true)
                {
                    foreach (var choice in choices)
                    {
                        defaultSetting.Add(choice.Value, 0);
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
                    return gameService.UpdateSlotConfig(new UpdateSlotConfigRequest(
                        request.GameInfo.Name,
                        request.BotSetting.BotEnabled,
                        request.GameInfo.EnablePromotionRTP,
                        request.SlotsConfig,
                        request.GameInfo.Type,
                        request.OtherGameIdsInGroup));

                case GameType.CascadingSlots:
                    return gameService
                        .UpdateCascadingSlotsConfig(new UpdateCascadingSlotsConfigRequest(
                            request.GameInfo.Name,
                            request.BotSetting.BotEnabled,
                            request.GameInfo.EnablePromotionRTP,
                            request.CascadingSlotsConfig,
                            request.GameInfo.Type,
                            request.OtherGameIdsInGroup));

                case GameType.SlotRNG:
                    return gameService.UpdateSlotRNGConfig(new UpdateSlotRNGConfigRequest(request.GameInfo, request.BotSetting, request.EnableSetting, request.SlotRNGConfig, request.OtherGameIdsInGroup));

                case GameType.Crash:
                    return gameService.UpdateCrashConfig(new UpdateCrashConfigRequest(request.GameInfo, request.BotSetting, request.EnableSetting, request.CrashConfig, request.OtherGameIdsInGroup));

                default:
                    return gameService.UpdateGeneralConfig(new UpdateGeneralConfigRequest(request.GameInfo, request.BotSetting, request.EnableSetting, request.OddsMultipliers, request.OtherGameIdsInGroup, request.TournamentConfig, request.VideoStreamConfig));
            }
        }

        public Task<GameCreationResult> Handle(CreateRequest request, CancellationToken cancellationToken)
        {
            var gameConfig = new GameConfig(
                string.Empty,
                request.Odds,
                request.GameInfo,
                request.EnableSetting,
                request.BotSettings,
                request.BetLimit,
                request.GameInfo.Type == GameType.Slots ? request.SlotConfigs : null,
                request.GameInfo.Type == GameType.CascadingSlots ? request.CascadingSlotsConfig : null,
                request.GameInfo.Type == GameType.SlotRNG ? request.SlotRNGConfig : null,
                null, // CrashConfig
                null, // OddsMultipliersConfig
                null, // TournamentConfig
                null, // VideoStreamConfig
                null, //NOSONAR
                request.OddsDisplayNames,
                request.Choices);

            return gameService.Add(gameConfig);
        }

        public Task<bool> Handle(IsExistRequest request, CancellationToken cancellationToken)
        => gameService.IsExistName(request.Name);

        public Task<bool> Handle(UpdateBotRatioRequest request, CancellationToken cancellationToken)
        => gameService.UpdateBotRatios(request);

        public async Task<StatusViewModel> Handle(GetStatusRequest request, CancellationToken cancellationToken)
        {
            var gameConfigs = await gameService.GetAll();
            var partners = await partnerService.GetAll();
            var currencyGroups = await currencyGroupService.GetAll();
            var currencyGroupGames = await currencyGroupGameService.GetAll();

            var statusViewModel = gameConfigs?.Any() == true
                ? new StatusViewModel(
                    gameConfigs.Select(game => new StatusItemViewModel(game))
                        .OrderBy(game => game.DisplayName)
                        .ToList(),
                    partners.ToList(),
                    currencyGroups.Where(group => group.Enabled).ToList(),
                    currencyGroupGames.ToList())
                : new StatusViewModel(new List<StatusItemViewModel>());

            return statusViewModel;
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
                    return gameService.UpdateCascadingSlotOdds(request.Name, request.CascadingSlotsConfig, request.OtherGameIdsInGroup);

                case GameType.Slots:
                    return gameService.UpdateSlotIconOdds(request.Name, request.ClassicIcons, request.OtherGameIdsInGroup);

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

        public Task<bool> Handle(UpdateSlotBotConfigRequest request, CancellationToken cancellationToken)
        => gameService.UpdateSlotBotConfig(request);

        public Task<Dictionary<string, string>> Handle(GetDisabledMessageRequest request, CancellationToken cancellationToken)
        => gameService.GetDisabledMessage(request.Name);

        public async Task<string> Handle(CloneSlotGameRequest request, CancellationToken cancellationToken)
        => await gameService.CloneNewSlotGame(request.GameId, request.ClonedFrom, request.DisplayName, request.GameType);

        public Task<bool> Handle(UpdateSymbolWeightByModeRequest request, CancellationToken cancellationToken)
        {
            var updateSymbolWeightRequest = new UpdateSymbolWeightRequest(
                    request.Name,
                    new SymbolWeight(request.Mode, request.Weights),
                    request.OtherGameIdsInGroup);
            return gameService.UpdateSymbolWeight(updateSymbolWeightRequest);
        }

        public Task<RtpResult> Handle(GetSlotRtpRequest request, CancellationToken cancellationToken)
        => gameService.GetSlotRtp(request);

        public async Task<StatusGameResult> Handle(CheckServiceStatusRequest request, CancellationToken cancellationToken)
        => await gameService.CheckServiceStatus(request);

        public async Task<string> Handle(UpdatePlinkoConfigRequest request, CancellationToken cancellationToken)
        => await gameService.UpdatePlinkoConfig(request);

    }
}
