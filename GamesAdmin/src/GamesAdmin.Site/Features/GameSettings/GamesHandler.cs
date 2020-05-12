using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site._Shared.Enumerations;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Account;
using GamesAdmin.Site.Features.GameSettings.Requests;
using GamesAdmin.Site.Features.GameSettings.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Refit;
using Sentry;
using Sentry.Protocol;
using GameType = GamesAdmin.Site._Shared.Enumerations.GameType;

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
        , IRequestHandler<UpdateStatusRequest, bool>
        , IRequestHandler<UpdateMultipleGameStatusRequest, bool>
        , IRequestHandler<UpdateDisabledMessageRequest, bool>
        , IRequestHandler<UpdateAllDisabledMessageRequest, bool>
        , IRequestHandler<ReloadGameClientRequest, bool>
    {
        private const int MaxLengthOfMessage = 1000;
        private const string DefaultDisabledMessage = "Game đang được bảo trì, vui lòng quay lại sau 10 phút.";

        private readonly IMapper mapper;

        private readonly IGameSettingService gameService;
        private readonly IAccountService accountService;
        private readonly IGameServerService gameServerService;
        private readonly IAppSettings appSettings;

        public GamesHandler(
            IMapper mapper,
            IGameSettingService gameService,
            IAccountService accountService,
            IGameServerService gameServerService,
            IAppSettings appSettings)
        {
            this.mapper = mapper;
            this.gameService = gameService;
            this.accountService = accountService;
            this.gameServerService = gameServerService;
            this.appSettings = appSettings;
        }

        public async Task<AddViewModel> Handle(AddRequest request, CancellationToken cancellationToken)
        {
            var viewModel = new AddViewModel
            {
                BotPercentageItems = GeneratePercentageItems(0)
            };

            var gameList = (await gameService.GetAll()).Select(game => game.Name).ToArray();
            viewModel.GameTypeItems = Enumeration.GetAll<GameType>()
                .Where(type => !gameList.Contains(type.Value))
                .Select(type => new SelectListItem(type.DisplayName, type.Value))
                .ToList();

            return viewModel;
        }

        public async Task<EditViewModel> Handle(EditRequest request, CancellationToken cancellationToken)
        {
            GameConfig game;

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                game = (await gameService.GetAll()).FirstOrDefault();
            }
            else
            {
                game = await gameService.GetByName(request.Name);
            }

            var viewModel = mapper.Map<EditViewModel>(game);

            if (viewModel == null)
            {
                viewModel = new EditViewModel();
            }

            viewModel.MaxBot = game == null ? 0 : (int)(game.MaxBot * 100);
            viewModel.BotMaxBet = game == null || game.BotMaxBet == 0 ? 10 : game.BotMaxBet;
            viewModel.BotCount = await accountService.GetBotCount();
            viewModel.BotRatioItems = GenerateBotRatioItems(game?.HoursMaxBot, viewModel.BotCount, viewModel.MaxBot);
            viewModel.BetChoiceBetSettings = GenerateBetSettings(game?.Name);

            return viewModel;
        }

        private static List<SelectListItem> GeneratePercentageItems(double currentValue)
        {
            var items = new List<SelectListItem>
            {
                new SelectListItem($"0.2%", $"0.2", currentValue == 0.2)
            };

            for (var i = 1; i <= 10; i++)
            {
                items.Add(new SelectListItem($"{i}%", $"{i}", currentValue == i));
            }

            for (var i = 15; i <= 100; i += 5)
            {
                var percentage = i;
                items.Add(new SelectListItem($"{percentage}%", $"{percentage}", currentValue == percentage));
            }

            return items;
        }

        private static List<BotRatioItemViewModel> GenerateBotRatioItems(double[] botRatios, int totalBot, double defaultRatio)
        {
            var botRatioItems = new List<BotRatioItemViewModel>();

            var hours = Enumerable.Range(00, 24)
                .Select(i => (DateTime.UtcNow.AddHours(i)));

            foreach (var hour in hours)
            {
                botRatioItems.Add(new BotRatioItemViewModel(
                    hour.Hour,
                    hour.AddHours(-4).Hour,
                    hour.AddHours(7).Hour,
                    botRatios == null || botRatios.Length == 0 ? defaultRatio : (int)(botRatios[hour.Hour] * 100),
                    totalBot));
            }

            return botRatioItems.OrderBy(item => item.GreenwichHour).ToList();
        }

        private Dictionary<string, decimal> GenerateBetSettings(string name)
        {
            var items = new Dictionary<string, decimal>();

            if (string.IsNullOrWhiteSpace(name))
            {
                return items;
            }

            var gameType = Enumeration.FromValue<GameType>(name);

            switch (gameType.Value)
            {
                case GameType.ShakeThePlateValue:
                    items = appSettings.BetSettings.ShakeThePlate;
                    break;

                case GameType.DragonTigerValue:
                    items = appSettings.BetSettings.MiniDragonTiger;
                    break;
            }

            return items;
        }

        //TODO update maxbet per choice
        public Task<bool> Handle(UpdateRequest request, CancellationToken cancellationToken)
        => gameService.Update(new GameConfig(
            request.Game.Id,
            request.Game.Name,
            request.Game.MinBet,
            request.Game.MaxBet,
            request.Game.Enabled,
            request.Game.BotEnabled,
            request.Game.MaxBot / 100,
            null,
            request.Game.BotMinBet,
            request.Game.BotMaxBet,
            request.Game.HoursMaxBot,
            request.Game.DisabledMessage,
            new Dictionary<string, decimal>()));

        //TODO update maxbet per choice
        public Task<bool> Handle(CreateRequest request, CancellationToken cancellationToken)
        => gameService.Add(new GameConfig(
            request.Game.Id,
            request.Game.Name,
            request.Game.MinBet,
            request.Game.MaxBet,
            request.Game.Enabled,
            request.Game.BotEnabled,
            request.Game.MaxBot / 100,
            null,
            0,
            0,
            new double[] { },
            DefaultDisabledMessage,
            new Dictionary<string, decimal>()));

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

        public Task<bool> Handle(UpdateStatusRequest request, CancellationToken cancellationToken)
        => gameService.UpdateStatus(request.Name, request.Enabled);

        public async Task<bool> Handle(UpdateMultipleGameStatusRequest request, CancellationToken cancellationToken)
        {
            if (request.Names?.Length == 0)
            {
                return false;
            }

            foreach (var name in request.Names)
            {
                await gameService.UpdateStatus(name, request.Enabled);
            }

            if (request.Reload)
            {
                await gameServerService.Reload();
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

        public async Task<bool> Handle(ReloadGameClientRequest request, CancellationToken cancellationToken)
        {
            await gameServerService.Reload();

            return true;
        }
    }
}
