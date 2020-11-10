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
        , IRequestHandler<DeleteRequest, bool>
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
    {
        private const int MaxLengthOfMessage = 1000;
        private const string DefaultDisabledMessage = "Game đang được bảo trì, vui lòng quay lại sau 10 phút.";
        private const int DefaultBotMaxBet = 10;

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
            var types = Enum.GetValues(typeof(GameType))
                .Cast<GameType>()
                .Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            var dificultyItems = Enum.GetValues(typeof(SlotDifficulties))
                .Cast<SlotDifficulties>()
                .Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            return Task.FromResult(new AddViewModel(types, dificultyItems, GenerateEmptyStakes()));
        }

        private static List<StakeLevel> GenerateEmptyStakes()
        => new List<StakeLevel> {
            new StakeLevel(0, 0),
            new StakeLevel(0, 0),
            new StakeLevel(0, 0)
        };

        public async Task<EditViewModel> Handle(EditRequest request, CancellationToken cancellationToken)
        {
            var game = string.IsNullOrWhiteSpace(request.Name)
                ? (await gameService.GetAll()).FirstOrDefault()
                : await gameService.GetByName(request.Name);

            var viewModel = mapper.Map<EditViewModel>(game);

            if (viewModel == null)
            {
                viewModel = new EditViewModel();
            }

            if (game != null)
            {
                viewModel.BotMaxBet = game.BotMaxBet <= 0 ? DefaultBotMaxBet : game.BotMaxBet;
                viewModel.BotCount = await accountService.GetBotCount();
                viewModel.BotRatioItems = GenerateBotRatioItems(game.HoursMaxBot, viewModel.BotCount);
                viewModel.BetChoiceBetSettings = GenerateBetSettings(game.Name, game.MaxBetChoices, game.MaxBet);
                viewModel.BetChoiceOdds = GenerateOdds(game.Name, game.Odds);
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

        private List<MaxBetChoiceViewModel> GenerateBetSettings(string name, Dictionary<string, double> maxBetChoices, double maxbet)
        {
            var items = new List<MaxBetChoiceViewModel>();

            if (string.IsNullOrWhiteSpace(name))
            {
                return items;
            }

            var gameType = Enumeration.FromValue<Game>(name);
            var betChoices = BetChoiceMapper.Map(gameType);

            if (betChoices != default)
            {
                foreach (var choice in betChoices)
                {
                    var maxBet = maxBetChoices.ContainsKey(choice.Value) ? maxBetChoices[choice.Value] : maxbet;
                    items.Add(new MaxBetChoiceViewModel(choice.Value, maxBet, choice.DisplayName));
                }
            }

            return items;
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

        public Task<bool> Handle(UpdateRequest request, CancellationToken cancellationToken)
        => gameService.Update(new AutoRNGGameConfig(
            request.Game.Id,
            request.Game.Type,
            request.Game.Name,
            request.Game.DisplayName,
            request.Game.MinBet,
            request.Game.MaxBet,
            request.Game.Enabled,
            request.Game.BotEnabled,
            request.Game.DisabledRound,
            request.Game.BotMaxBet,
            request.Game.HoursMaxBot,
            request.Game.DisabledMessage,
            request.Game.MaxBetChoices,
            request.Game.Odds,
            request.Game.DelayStartTime,
            request.Game.EnableFreeBet));

        public Task<bool> Handle(CreateRequest request, CancellationToken cancellationToken)
        => gameService.Add(new AutoRNGGameConfig(
            request.Game.Id,
            request.Game.Type,
            request.Game.Name,
            request.Game.DisplayName,
            request.Game.MinBet,
            request.Game.MaxBet,
            request.Game.Enabled,
            request.Game.BotEnabled,
            null,
            0,
            new double[] { },
            DefaultDisabledMessage,
            new Dictionary<string, double>(),
            new Dictionary<string, double>(),
            request.Game.DelayStartTime,
            request.Game.EnableFreeBet));

        public Task<bool> Handle(DeleteRequest request, CancellationToken cancellationToken)
        => gameService.Delete(request.Id);

        public Task<bool> Handle(IsExistRequest request, CancellationToken cancellationToken)
        => gameService.IsExistName(request.Name);

        public async Task<bool> Handle(UpdateBotRatioRequest request, CancellationToken cancellationToken)
        {
            var game = await gameService.GetByName(request.Name);
            game.HoursMaxBot = request.BotRatios;

            return await gameService.Update(game);
        }

        public async Task<StatusViewModel> Handle(GetStatusRequest request, CancellationToken cancellationToken)
        {
            var gameConfigs = await gameService.GetAll();
            var gameList = new List<StatusItemViewModel>();

            if (gameConfigs?.Any() == true)
            {
                gameList = gameConfigs
                    .Select(game => mapper.Map<StatusItemViewModel>(game))
                    .OrderBy(game => game.DisplayName)
                    .ToList();
            }

            return new StatusViewModel(gameList);
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

            var games = await gameService.GetAll();

            foreach (var game in games)
            {
                await gameService.UpdateDisabledMessage(game.Name, request.DisabledMessage);
            }

            return true;
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
    }
}