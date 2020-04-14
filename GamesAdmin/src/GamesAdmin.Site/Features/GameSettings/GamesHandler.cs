using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Site._Shared.Enumerations;
using GamesAdmin.Site.Features.Account;
using GamesAdmin.Site.Features.GameSettings.Requests;
using GamesAdmin.Site.Features.GameSettings.ViewModels;
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
    {
        private readonly IMapper mapper;

        private readonly IGameSettingService gameService;
        private readonly IAccountService accountService;

        public GamesHandler(
            IMapper mapper,
            IGameSettingService gameService,
            IAccountService accountService)
        {
            this.mapper = mapper;
            this.gameService = gameService;
            this.accountService = accountService;
        }

        public async Task<AddViewModel> Handle(AddRequest request, CancellationToken cancellationToken)
        {
            var viewModel = new AddViewModel {
                BotPercentageItems = GeneratePercentageItems(0)
            };

            var gameList = (await gameService.GetGames()).Select(game => game.Name).ToArray();
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
                game = (await gameService.GetGames()).FirstOrDefault();
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
            viewModel.BotPercentageItems = GeneratePercentageItems(viewModel.MaxBot);
            viewModel.BotRatioItems = GenerateBotRatioItems(game?.HoursMaxBot, viewModel.BotCount, viewModel.MaxBot);

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
            request.Game.HoursMaxBot));

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
            new double[] { }));

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
    }
}
