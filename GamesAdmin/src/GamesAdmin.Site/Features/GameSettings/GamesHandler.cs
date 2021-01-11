using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site.Extensions;
using GamesAdmin.Site.Features.GameSettings.Requests;
using GamesAdmin.Site.Features.GameSettings.ViewModels;
using GamesAdmin.Site.Features.GameUsers;
using GamesAdmin.Site.Features.ProVietKeno;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.GameSettings
{
    public class GamesHandler
        : IRequestHandler<EditRequest, EditViewModel>
        , IRequestHandler<UpdateRequest, bool>
        , IRequestHandler<AddRequest, AddViewModel>
        , IRequestHandler<CreateRequest, bool>
        , IRequestHandler<IsExistRequest, bool>
        , IRequestHandler<UpdateBotRatioRequest, bool>
        , IRequestHandler<GetStatusRequest, StatusViewModel>
        , IRequestHandler<UpdateMultipleGameStatusRequest, bool>
        , IRequestHandler<UpdateDisabledMessageRequest, bool>
        , IRequestHandler<UpdateAllDisabledMessageRequest, bool>
        , IRequestHandler<ReloadGameClientRequest, bool>
        , IRequestHandler<GetJWTRequest, GameServerToken>
        , IRequestHandler<GetMonitorRequest, MonitorViewModel>
        , IRequestHandler<UnderMaintenanceRequest, bool>
        , IRequestHandler<UpdateBetLimitAndOddsRequest, bool>
        , IRequestHandler<UpdateJackpotRequest, bool>
    {
        private const int MaxLengthOfMessage = 1000;

        private readonly IMapper mapper;

        private readonly IGameSettingService gameService;
        private readonly IGameUserService accountService;
        private readonly IProVietKenoService proVietKenoService;
        private readonly IAppSettings appSettings;

        public GamesHandler(
            IMapper mapper,
            IGameSettingService gameService,
            IGameUserService accountService,
            IProVietKenoService proVietKenoService,
            IAppSettings appSettings)
        {
            this.mapper = mapper;
            this.gameService = gameService;
            this.accountService = accountService;
            this.proVietKenoService = proVietKenoService;
            this.appSettings = appSettings;
        }

        public Task<AddViewModel> Handle(AddRequest request, CancellationToken cancellationToken)
        {
            var names = Enumeration.GetAll<Game>()
                .Select(x => new SelectListItem { Text = x.DisplayName, Value = x.Value }).ToList();

            var types = Enum.GetValues(typeof(GameType))
                .Cast<GameType>()
                .Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            var difficultyItems = Enum.GetValues(typeof(GameMode))
                .Cast<GameMode>()
                .Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            return Task.FromResult(new AddViewModel(names, types, difficultyItems));
        }

        public async Task<EditViewModel> Handle(EditRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return new EditViewModel();
            }

            var game = await gameService.GetByName(request.Name);
            var difficultyItems = Enum.GetValues(typeof(GameMode))
                .Cast<GameMode>()
                .Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            var viewModel = mapper.Map<EditViewModel>(game);

            if (game != null)
            {
                viewModel.PayoutViewModel = mapper.Map<PayoutViewModel>(game);

                viewModel.BotMaxBet = game.BotSetting.BotMaxBet;
                viewModel.BotCount = await accountService.GetBotCount();
                viewModel.BotRatioItems = GenerateBotRatioItems(game.BotSetting.HoursMaxBot, viewModel.BotCount);
                viewModel.DisplayName = Enumeration.Contains<Game>(game.Info.Name)
                                        ? Enumeration.FromValue<Game>(game.Info.Name).DisplayName
                                        : game.Info.Name;

                if (game.Info.Type == GameType.Slots)
                {
                    viewModel.SlotViewModel = mapper.Map<SlotViewModel>(game.SlotsConfig);
                    viewModel.SlotViewModel.ModeItems = difficultyItems;
                    viewModel.PayoutViewModel.IconOdds = GenerateIconOdds(game.SlotsConfig.Icons);

                    viewModel.JackpotViewModel = mapper.Map<JackpotViewModel>(game.SlotsConfig);
                }
                else
                {
                    viewModel.Type = GameType.AutoRNG;
                    var choices = GetBetChoices(game.Info.Name);

                    viewModel.PayoutViewModel.BetChoiceBetSettings = GenerateBetSettings(choices, game.BetLimit);
                    viewModel.PayoutViewModel.BetChoiceOdds = GenerateOdds(game.Info.Name, game.Odds);
                }
            }

            return viewModel;
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
                betLimit.MaxBetChoices.ContainsKey(choice.Value) ? betLimit.MaxBetChoices[choice.Value] : betLimit.MaxBet,
                choice.DisplayName)).ToList();
        }

        private List<OddsViewModel> GenerateOdds(string name, Dictionary<string, double> oddsSettings)
        {
            var items = new List<OddsViewModel>();

            if (string.IsNullOrWhiteSpace(name))
            {
                return items;
            }

            var gameType = Enumeration.FromValue<Game>(name);
            var betOdds = BetOddsMapper.Map(gameType);

            if (betOdds != default)
            {
                foreach (var odd in betOdds)
                {
                    var odds = oddsSettings.ContainsKey(odd.Value) ? oddsSettings[odd.Value] : 0;
                    items.Add(new OddsViewModel(odd.Value, odds, odd.DisplayName));
                }
            }

            return items;
        }

        private List<IconOddsViewModel> GenerateIconOdds(Dictionary<string, Dictionary<string, double>> iconOdds)
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
            if (string.IsNullOrWhiteSpace(name) || !Enumeration.Contains<Game>(name))
            {
                return default;
            }

            return BetChoiceMapper.Map(Enumeration.FromValue<Game>(name));
        }

        public Task<bool> Handle(UpdateRequest request, CancellationToken cancellationToken)
        => request.GameInfo.Type == GameType.AutoRNG
                ? gameService.UpdateGeneralConfig(new UpdateGeneralConfigRequest(request.GameInfo, request.BotSetting, request.EnableSetting))
                : gameService.UpdateSlotConfig(new UpdateSlotConfigRequest(request.GameInfo.Name, request.BotSetting.BotEnabled, request.SlotsConfig));

        public Task<bool> Handle(CreateRequest request, CancellationToken cancellationToken)
        => gameService.Add(new GameConfig(
            string.Empty,
            new Dictionary<string, double>(),
            request.GameInfo,
            request.EnableSetting,
            request.BotSettings,
            request.BetLimit,
            request.SlotConfigs));

        public Task<bool> Handle(IsExistRequest request, CancellationToken cancellationToken)
        => gameService.IsExistName(request.Name);

        public Task<bool> Handle(UpdateBotRatioRequest request, CancellationToken cancellationToken)
        => gameService.UpdateBotRatios(request.Name, request.BotRatios);

        public async Task<StatusViewModel> Handle(GetStatusRequest request, CancellationToken cancellationToken)
        {
            var gameConfigs = await gameService.GetAll();

            return gameConfigs?.Any() == true
                ? new StatusViewModel(gameConfigs
                    .Select(game => new StatusItemViewModel(game))
                    .OrderBy(game => game.DisplayName)
                    .ToList())
                : new StatusViewModel(new List<StatusItemViewModel>());
        }

        public async Task<bool> Handle(UpdateMultipleGameStatusRequest request, CancellationToken cancellationToken)
        {
            if (request.Names?.Length == 0)
            {
                return false;
            }

            await gameService.UpdateStatus(request.Names, request.Enabled);

            if (request.Names.Any(x => x == Game.KenoProVietValue) && request.Enabled)
            {
                await NotifyUnsettleRoundsToGameServer();
            }

            if (request.Reload)
            {
                await gameService.Reload();
            }

            return true;
        }

        public async Task<bool> Handle(UpdateDisabledMessageRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name) || request.DisabledMessage?.Length > MaxLengthOfMessage)
            {
                return false;
            }

            return await gameService.UpdateDisabledMessage(request.Name, request.DisabledMessage);
        }

        public async Task<bool> Handle(UpdateAllDisabledMessageRequest request, CancellationToken cancellationToken)
        {
            if (request.DisabledMessage?.Length > MaxLengthOfMessage)
            {
                return false;
            }

            return await gameService.UpdateDisabledMessage(string.Empty, request.DisabledMessage, true);
        }

        public Task<bool> Handle(ReloadGameClientRequest request, CancellationToken cancellationToken)
        => gameService.Reload();

        public async Task<GameServerToken> Handle(GetJWTRequest request, CancellationToken cancellationToken)
        {
            var result = await gameService.GetJWT(request.Username, request.UserAgent);
            var redirectUrl = @$"/game-admin/monitor.html?game={request.Game}&v={DateTime.Now.Ticks}";
            return new GameServerToken
            (
                result.Token,
                appSettings.Env,
                redirectUrl
            );
        }

        private async Task NotifyUnsettleRoundsToGameServer()
        {
            var unsettleRounds = await proVietKenoService.GetUnsettledRoundsAsync();
            await proVietKenoService.NotifyRoundResult(unsettleRounds);
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

            return new MonitorViewModel(gameList);
        }

        public Task<bool> Handle(UnderMaintenanceRequest request, CancellationToken cancellationToken)
        => gameService.UnderMaintenance(request);

        public Task<bool> Handle(UpdateBetLimitAndOddsRequest request, CancellationToken cancellationToken)
        => request.Type == GameType.AutoRNG
                ? gameService.UpdateBetLimitAndOdds(request)
                : gameService.UpdateSlotIconOdds(request.Name, request.Icons);

        public Task<bool> Handle(UpdateJackpotRequest request, CancellationToken cancellationToken)
        => gameService.UpdateJackpot(request);
    }
}