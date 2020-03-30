using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features.Account;
using GamesAdmin.Site.Features.GameSettings.Requests;
using GamesAdmin.Site.Features.GameSettings.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sentry;

namespace GamesAdmin.Site.Features.GameSettings
{
    public class GamesHandler
        : IRequestHandler<EditRequest, EditViewModel>
        , IRequestHandler<UpdateRequest, bool>
        , IRequestHandler<AddRequest, AddViewModel>
        , IRequestHandler<CreateRequest, bool>
        , IRequestHandler<DeleteRequest, bool>
        , IRequestHandler<IsExistRequest, bool>
    {
        private readonly IMapper mapper;

        private readonly IGameService gameService;
        private readonly IAccountService accountService;
        private readonly ISentryClient sentryClient;

        public GamesHandler(
            IMapper mapper, 
            IGameService gameService, 
            IAccountService accountService,
            ISentryClient sentryClient)
        {
            this.mapper = mapper;
            this.gameService = gameService;
            this.accountService = accountService;
            this.sentryClient = sentryClient;
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

            if (game == null)
            {
                sentryClient.CaptureMessage("Edit game is null");
            }

            if (viewModel == null)
            {
                sentryClient.CaptureMessage("viewModel is null");
                viewModel = new EditViewModel();
            }

            viewModel.MaxBot = game == null ? 0 : (int)(game.MaxBot * 100);
            viewModel.BotCount = await accountService.GetBotCount();

            if (viewModel.BotCount == 0)
            {
                sentryClient.CaptureMessage("Bot count is zero");
            }

            viewModel.BotPercentageItems = GeneratePercentageItems(viewModel.MaxBot);

            return viewModel;
        }

        private static List<SelectListItem> GeneratePercentageItems(int currentValue)
        {
            var items = new List<SelectListItem>();

            for (var i = 1; i <= 10; i++)
            {
                var percentage = i * 10;
                items.Add(new SelectListItem($"{percentage}%", $"{percentage}", currentValue == percentage));
            }

            return items;
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
            null));

        public async Task<AddViewModel> Handle(AddRequest request, CancellationToken cancellationToken)
        {
            var viewModel = new AddViewModel
            {
                BotCount = await accountService.GetBotCount()
            };

            viewModel.BotPercentageItems = GeneratePercentageItems(viewModel.MaxBot);

            return viewModel;
        }

        public Task<bool> Handle(CreateRequest request, CancellationToken cancellationToken)
        => gameService.Add(new GameConfig(
            request.Game.Id,
            request.Game.Name,
            request.Game.MinBet,
            request.Game.MaxBet,
            request.Game.Enabled,
            request.Game.BotEnabled,
            request.Game.MaxBot / 100,
            null));

        public Task<bool> Handle(DeleteRequest request, CancellationToken cancellationToken)
        => gameService.Delete(request.Id);

        public Task<bool> Handle(IsExistRequest request, CancellationToken cancellationToken)
        => gameService.IsExistName(request.Name);
    }
}
