using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.Partners;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.OperateSettings.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Refit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Site.Features.GameSettings.Requests;
using GamesAdmin.Site.Features.GameSettings;
using GamesAdmin.Site.Features.OperateSettings.Responses;
using System;
using Sentry;
using GamesAdmin.Site.Features.Partners;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Site.Features.Currencies;
using GamesAdmin.Site.Features.OperateSettings.ViewModels;
using GamesAdmin.Site.Features.CurrencyGroupGames;
using GamesAdmin.Site.Features.GameSettings.ViewModels;
using GamesAdmin.Site.Features.CurrencyGroups;

namespace GamesAdmin.Site.Features.OperateSettings
{
    public interface IOperateSettingsApi : IBaseAuthorizationApi
    {
        [Post("/operate_settings/game/settings")]
        Task<IEnumerable<GameConfig>> GetGameSettings([Body]IList<int> gameIds);
    } 

    public interface IOperateSettingsService
    {
        Task<IEnumerable<GameConfig>> GetGameSettings(IList<int> gameIds);

        Task<ImportGameSettingResponse> ImportGameSetting(ImportGameSettingRequest request);

        Task<IEnumerable<CurrencyByOperatorViewModel>> GetOperatorCurrencies();

        Task<Dictionary<string, MessageResult>> ImportCurrencies(ImportCurrenciesRequest request);

        Task<IEnumerable<CurrencyGroupGame>> GetCurrencyGroupGames();
        
        Task<StatusViewModel> GetAllGames();
    }

    public class OperateSettingsService : IOperateSettingsService
    {
        private readonly IOperateSettingsApi operateSettingsApi;
        private readonly IGameSettingService gameSettingService;
        private readonly ISentryClient sentryClient;
        private readonly IPartnerService partnerService;
        private readonly ICurrencyService currencyService;
        private readonly IMediator mediator;
        private readonly ICurrencyGroupGameService currencyGroupGameService;
        private readonly ICurrencyGroupService currencyGroupService;

        public OperateSettingsService(
            IOperateSettingsApi operateSettingsApi,
            IGameSettingService gameSettingService,
            ISentryClient sentryClient,
            IPartnerService partnerService,
            ICurrencyService currencyService,
            IMediator mediator,
            ICurrencyGroupGameService currencyGroupGameService,
            ICurrencyGroupService currencyGroupService) {
            this.operateSettingsApi = operateSettingsApi;
            this.gameSettingService = gameSettingService;
            this.sentryClient = sentryClient;
            this.partnerService = partnerService;
            this.currencyService = currencyService;
            this.mediator = mediator;
            this.currencyGroupGameService = currencyGroupGameService;
            this.currencyGroupService = currencyGroupService;
        }

        public async Task<IEnumerable<GameConfig>> GetGameSettings(IList<int> gameIds)
        {
            var allGameSettings = await this.operateSettingsApi.GetGameSettings(gameIds);

            return allGameSettings;
        }

        public async Task<ImportGameSettingResponse> ImportGameSetting(ImportGameSettingRequest request)
        {
            var importSettingResponse = new ImportGameSettingResponse();

            if (request.GameSettings?.Count() > 0)
            {
                foreach (var gameSetting in request.GameSettings)
                {
                    try
                    {
                        var gameConfig = new GameConfig(
                            gameSetting.GameId.ToString(),
                            gameSetting.Odds,
                            gameSetting.Info,
                            gameSetting.EnableSetting,
                            gameSetting.BotSetting,
                            gameSetting.BetLimit,
                            gameSetting.SlotsConfig,
                            gameSetting.CascadingSlotsConfig,
                            gameSetting.SlotRNGConfig,
                            gameSetting.CrashConfig);

                        gameConfig.BotSetting = gameSetting.BotSetting;
                        gameConfig.EnableSetting = gameSetting.EnableSetting;
                        gameConfig.Margin = gameSetting.Margin;

                        var isSuccesfully = await gameSettingService.Import(gameConfig);

                        importSettingResponse.ImportResult.Add(gameSetting.GameId, isSuccesfully);
                    }
                    catch (Exception ex)
                    {
                        sentryClient.CaptureException(ex);

                        importSettingResponse.ImportResult.Add(gameSetting.GameId, false);
                    }
                }
            }

            return importSettingResponse;
        }

        public async Task<IEnumerable<CurrencyByOperatorViewModel>> GetOperatorCurrencies()
        {
            var operators = await partnerService.GetAll();
            var operatorCurrencies = operators.Select(op => new CurrencyByOperatorViewModel { Operator = op }).ToList();
            var allCurrencies = (await currencyService.GetAll()).ToList();

            foreach (var opCurrency in operatorCurrencies) {
                var currencies = allCurrencies.Where(currency => currency.PartnerId == opCurrency.Operator.PartnerId).ToList();
                opCurrency.Currencies = currencies;
            }

            return operatorCurrencies;
        }

        public async Task<Dictionary<string, MessageResult>> ImportCurrencies(ImportCurrenciesRequest request)
        {
            var results = new Dictionary<string, MessageResult>();

            foreach (var operatorCurrency in request.OperatorCurrencies)
            {
                foreach (var currency in operatorCurrency.Currencies)
                {
                    var createCurrencyRequest = new Currencies.Requests.CreateRequest(new Currencies.ViewModels.CreateCurrencyViewModel
                    {
                        Code = currency.Code,
                        Enabled = currency.Enabled,
                        IsTest = currency.IsTest,
                        IsUncountOnTotal = currency.IsUncountOnTotal,
                        Multiplier = currency.Multiplier,
                        PartnerId = operatorCurrency.Operator,
                        Name = currency.Name
                    });

                    var createResult = await mediator.Send(createCurrencyRequest);

                    results.Add($"{operatorCurrency.Operator} - {currency.Name}", createResult);
                }
            }

            return results;
        }

        public async Task<IEnumerable<CurrencyGroupGame>> GetCurrencyGroupGames()
        {
            var allCurrencyGroupGames = await this.currencyGroupGameService.GetAll();

            return allCurrencyGroupGames;
        }

        public async Task<StatusViewModel> GetAllGames()
        {
            var statusViewModel = await mediator.Send(new GetStatusRequest());

            return statusViewModel;
        }

        public async Task<bool> ImportCurrencyGroupGame(ImportCurrencyGroupGameRequest request)
        {
            var results = new Dictionary<string, MessageResult>();
            var allCurrencyGroup = await this.currencyGroupService.GetAll();

            foreach (var currencyGroupGame in request.CurrencyGroupGameImportModels) {
                var currencyGroup = allCurrencyGroup.FirstOrDefault(
                    group => group.PartnerId == currencyGroupGame.OperatorId && group.Name == currencyGroupGame.GroupName);
                if (currencyGroup != null) {
                    foreach(var gameSetting in currencyGroupGame.GameSettings)
                    {
                        var updateResult = await this.currencyGroupGameService.Update(new CurrencyGroupGame(
                        currencyGroup.Id,
                        (int)gameSetting.GameId,
                        gameSetting.GameName,
                        false,
                        gameSetting.MinBet,
                        gameSetting.MaxBet,
                        gameSetting.MaxBetPerChoices,
                        gameSetting.Chips.ToArray()
                        ));
                    }
                }
            }
        }
    }
}
