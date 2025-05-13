using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site._Shared.Services.GeneralGameSettings;
using GamesAdmin.Site.Features.Currencies.Requests;
using GamesAdmin.Site.Features.GameSettings.ViewModels;
using GamesAdmin.Site.Features.JackpotSettingsManagement.Requests;
using GamesAdmin.Site.Features.JackpotSettingsManagement.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using GamesAdmin.Site.Features.GameUsers;
using GamesAdmin.Site.Extensions;

namespace GamesAdmin.Site.Features.JackpotSettingsManagement.Handlers
{
    public class JackpotSettingsHandler : 
        IRequestHandler<IndexPageRequest, JackpotSettingsPageViewModel>, 
        IRequestHandler<CreatePageRequest, JackpotSettingsViewModel>,
        IRequestHandler<UpdatePlayerSettingsRequest, UpdateJackpotSettingsResult>,
        IRequestHandler<UpdateBotSettingsRequest, UpdateJackpotSettingsResult>,
        IRequestHandler<UpdateBotRatioSettingsRequest, UpdateJackpotSettingsResult>
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IGeneralGameSettingService generalGameSettingService;
        private readonly IGameUserService gameUserService;
        private readonly IJackpotSettingApi jackpotSettingApi;

        public JackpotSettingsHandler(IMapper mapper, IMediator mediator, IGeneralGameSettingService generalGameSettingService, IGameUserService gameUserService, IJackpotSettingApi jackpotSettingApi)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.generalGameSettingService = generalGameSettingService;
            this.gameUserService = gameUserService;
            this.jackpotSettingApi = jackpotSettingApi;
        }

        public async Task<JackpotSettingsPageViewModel> Handle(IndexPageRequest request, CancellationToken cancellationToken)
        {
            var games = await generalGameSettingService.GetJackpotGames();

            return new JackpotSettingsPageViewModel
            {
                Games = games.Select(game => new SelectListItem(game.DisplayName, game.GameId.ToString(), true))
            };
        }

        public async Task<JackpotSettingsViewModel> Handle(CreatePageRequest request, CancellationToken cancellationToken)
        {
            var games = await generalGameSettingService.GetJackpotGames();
            var operators = await mediator.Send(new GetPartnersForSelectionRequest());
            var totalBot = await gameUserService.GetBotCount();

            return new JackpotSettingsViewModel
            {
                Pools = new JackpotPools
                {
                    Grand = new Pool(),
                    Major = new Pool(),
                    Minor = new Pool(),
                },
                Games = games.Select(game => new SelectListItem(game.DisplayName, game.GameId.ToString(), true)),
                Operators = operators,
                TotalBot = totalBot,
                BotRatios = GenerateBotRatioItems(Array.Empty<double>(), totalBot)
            };
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
                    botRatios.Any() ? botRatios[hour.Hour].ToPercentage() : 0,
                    totalBot));
            }

            return botRatioItems.OrderBy(item => item.GreenwichHour).ToList();
        }

        public async Task<UpdateJackpotSettingsResult> Handle(UpdatePlayerSettingsRequest request, CancellationToken cancellationToken)
        {
            var model = mapper.Map<JackpotSettings>(request.Settings);
            return await jackpotSettingApi.UpdatePlayerSettings(model);
        }

        public async Task<UpdateJackpotSettingsResult> Handle(UpdateBotSettingsRequest request, CancellationToken cancellationToken)
        {
            var model = mapper.Map<JackpotSettings>(request.Settings);
            return await jackpotSettingApi.UpdateBotSettings(model);
        }

        public async Task<UpdateJackpotSettingsResult> Handle(UpdateBotRatioSettingsRequest request, CancellationToken cancellationToken)
        {
            var model = mapper.Map<JackpotSettings>(request.Settings);
            return await jackpotSettingApi.UpdateBotRatioSettings(model);
        }
    }
}