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
using GamesAdmin.Core.Models;

namespace GamesAdmin.Site.Features.JackpotSettingsManagement.Handlers
{
    public class JackpotSettingsHandler :
        IRequestHandler<IndexPageRequest, JackpotSettingsPageViewModel>,
        IRequestHandler<CreatePageRequest, JackpotSettingsViewModel>,
        IRequestHandler<UpdatePlayerSettingsRequest, UpdateJackpotSettingsResult>,
        IRequestHandler<UpdateBotSettingsRequest, UpdateJackpotSettingsResult>,
        IRequestHandler<UpdateBotRatioSettingsRequest, UpdateJackpotSettingsResult>,
        IRequestHandler<CopyJackpotSettingsRequest, JackpotSettingsViewModel>,
        IRequestHandler<SearchRequest, IEnumerable<SearchResultViewModel>>,
        IRequestHandler<UpdatePageRequest, JackpotSettingsViewModel>
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
            var (games, operators) = await GetGamesAndOperators();
            var totalBot = await gameUserService.GetBotCount();
            var jackpotSettings = await jackpotSettingApi.GetAllJackpotSettings();

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
                BotRatios = GenerateBotRatioItems(Array.Empty<double>(), totalBot),
                OtherSettings = InitSettingsSelectionList(jackpotSettings, games, operators)
            };
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

        public async Task<JackpotSettingsViewModel> Handle(CopyJackpotSettingsRequest request, CancellationToken cancellationToken)
        {
            var jackpotSettings = await jackpotSettingApi.GetJackpotSettings(request.ConfigId);
            var totalBot = await gameUserService.GetBotCount();

            return new JackpotSettingsViewModel
            {
                Enabled = false,
                ExcludeTestCurrency = jackpotSettings.ExcludeTestCurrency,
                WinningRatio = jackpotSettings.WinningRatio,
                Pools = jackpotSettings.Pools,
                BotBetLevels = jackpotSettings.BotBetLevels.ToList(),
                BotEnabled = false,
                TotalBot = totalBot,
                BotRatios = GenerateBotRatioItems(jackpotSettings.HoursMaxBot, totalBot),
            };
        }

        public async Task<IEnumerable<SearchResultViewModel>> Handle(SearchRequest request, CancellationToken cancellationToken)
        {
            var jackpotSettings = await jackpotSettingApi.Search(request.GameIds, request.SiteNames?.Split(';') ?? Enumerable.Empty<string>());

            if (jackpotSettings.Any())
            {
                var games = await generalGameSettingService.GetJackpotGames();
                var operators = await mediator.Send(new GetPartnersForSelectionRequest());

                var viewModels = jackpotSettings.Select(setting => new SearchResultViewModel
                {
                    Id = setting.Id,
                    GameName = games.FirstOrDefault(g => g.GameId == setting.GameId)?.DisplayName,
                    OperatorName = operators.FirstOrDefault(o => o.Value == setting.OperatorId)?.Text,
                    SiteNames = setting.SiteNames,
                    BotEnabled = setting.BotEnabled,
                    Enabled = setting.Enabled,
                    GameId = setting.GameId,
                    OperatorId = setting.OperatorId,
                }).OrderBy(x => x.GameName).ThenBy(x => x.OperatorName);

                return viewModels;
            }

            return Enumerable.Empty<SearchResultViewModel>();
        }
        public async Task<JackpotSettingsViewModel> Handle(UpdatePageRequest request, CancellationToken cancellationToken)
        {
            var (games, operators) = await GetGamesAndOperators();
            var jackpotSettings = await jackpotSettingApi.GetJackpotSettings(request.ConfigId);
            var totalBot = await gameUserService.GetBotCount();
            var otherJackpotSettings = await jackpotSettingApi.GetAllJackpotSettings();

            return new JackpotSettingsViewModel
            {
                Id = jackpotSettings.Id,
                GameId = jackpotSettings.GameId,
                OperatorId = jackpotSettings.OperatorId,
                SiteNames = string.Join(";", jackpotSettings.SiteNames),
                Enabled = jackpotSettings.Enabled,
                ExcludeTestCurrency = jackpotSettings.ExcludeTestCurrency,
                WinningRatio = jackpotSettings.WinningRatio,
                Pools = jackpotSettings.Pools,
                BotBetLevels = jackpotSettings.BotBetLevels.ToList(),
                BotEnabled = jackpotSettings.BotEnabled,
                TotalBot = totalBot,
                BotRatios = GenerateBotRatioItems(jackpotSettings.HoursMaxBot, totalBot),
                Games = games.Select(game => new SelectListItem(game.DisplayName, game.GameId.ToString(), true)),
                Operators = operators,
                OtherSettings = InitSettingsSelectionList(otherJackpotSettings, games, operators),
            };
        }

        private async Task<(IEnumerable<GeneralGameSetting> games, IEnumerable<SelectListItem> operators)> GetGamesAndOperators()
        {
            var games = await generalGameSettingService.GetJackpotGames();
            var operators = await mediator.Send(new GetPartnersForSelectionRequest());
            return (games, operators);
        }

        private IEnumerable<SelectListItem> InitSettingsSelectionList(IEnumerable<JackpotSettings> jackpotSettings,
        IEnumerable<GeneralGameSetting> games,
        IEnumerable<SelectListItem> operators)
        {
            return jackpotSettings.Select(setting =>
            {
                var game = games.FirstOrDefault(g => g.GameId == setting.GameId);
                var @operator = operators.FirstOrDefault(o => o.Value == setting.OperatorId);
                return new SelectListItem(
                    $"{game.DisplayName} - {@operator.Text} - {string.Join(", ", setting.SiteNames)}",
                    setting.Id);
            });
        }

        private static List<BotRatioItemViewModel> GenerateBotRatioItems(double[] botRatios, int totalBot)
        {
            return Enumerable.Range(0, 24)
                .Select(i => DateTime.UtcNow.AddHours(i))
                .Select(hour => new BotRatioItemViewModel(
                    hour.Hour,
                    hour.ToGreenwichHour(),
                    hour.ToAsiaHour(),
                    botRatios.Any() ? botRatios[hour.Hour].ToPercentage() : 0,
                    totalBot))
                .OrderBy(item => item.GreenwichHour)
                .ToList();
        }
    }
}