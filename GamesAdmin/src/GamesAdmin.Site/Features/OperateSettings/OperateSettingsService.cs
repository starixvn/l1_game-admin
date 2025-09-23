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
        Task<IEnumerable<GameConfig>> GetGameSettings([Body] IList<int> gameIds);
    }

    public interface IOperateSettingsService
    {
        Task<IEnumerable<GameConfig>> GetGameSettings(IList<int> gameIds);

        Task<ImportGameSettingResponse> ImportGameSetting(ImportGameSettingRequest request);

        Task<IEnumerable<CurrencyByOperatorViewModel>> GetOperatorCurrencies();

        Task<Dictionary<string, MessageResult>> ImportCurrencies(ImportCurrenciesRequest request);

        Task<IEnumerable<CurrencyGroupGame>> GetCurrencyGroupGames();

        Task<StatusViewModel> GetAllGames();

        Task<Dictionary<string, MessageResult>> ImportCurrencyGroupGame(ImportCurrencyGroupGameRequest request);
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
            ICurrencyGroupService currencyGroupService)
        {
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

            // Early return if no game settings to process
            if (request.GameSettings?.Any() != true)
            {
                return importSettingResponse;
            }

            foreach (var gameConfigImport in request.GameSettings)
            {
                var importResult = await ProcessGameSettingImport(gameConfigImport);
                importSettingResponse.ImportResult.Add(gameConfigImport.GameId, importResult);
            }

            return importSettingResponse;
        }

        private async Task<bool> ProcessGameSettingImport(GameConfigImport gameConfigImport)
        {
            try
            {
                if(gameConfigImport.GameId <= 0 || gameConfigImport.Info?.GameId <= 0)
                {
                    return false;
                }    

                // Convert GameConfigImport to GameConfig for the import service
                // Disable the game by default when importing if EnableSetting exists
                if (gameConfigImport.EnableSetting != null)
                {
                    gameConfigImport.EnableSetting.Enabled = false; // Disable the game by default when importing
                }


                return await gameSettingService.Import(gameConfigImport);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return false;
            }
        }

        public async Task<IEnumerable<CurrencyByOperatorViewModel>> GetOperatorCurrencies()
        {
            var operators = await partnerService.GetAll();
            var operatorCurrencies = operators.Select(op => new CurrencyByOperatorViewModel { Operator = op }).ToList();
            var allCurrencies = (await currencyService.GetAll()).ToList();

            foreach (var opCurrency in operatorCurrencies)
            {
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
                        Enabled = true,
                        IsTest = currency.IsTest,
                        IsUncountOnTotal = currency.IsUncountOnTotal,
                        Multiplier = currency.Multiplier,
                        PartnerId = operatorCurrency.Operator,
                        Name = currency.Name,
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

        public async Task<Dictionary<string, MessageResult>> ImportCurrencyGroupGame(ImportCurrencyGroupGameRequest request)
        {
            var results = new Dictionary<string, MessageResult>();
            var allCurrencyGroup = await this.currencyGroupService.GetAll();

            foreach (var currencyGroupGame in request.CurrencyGroupGameImportModels)
            {
                var currencyGroup = allCurrencyGroup.FirstOrDefault(
                    group => group.PartnerId == currencyGroupGame.OperatorId && group.Name == currencyGroupGame.GroupName);
                if (currencyGroup != null)
                {
                    foreach (var gameSetting in currencyGroupGame.GameSettings)
                    {
                        var currencyGroupName = $"{currencyGroupGame.OperatorId} - {currencyGroup.Name} - {gameSetting.GameName}";
                        try
                        {
                            gameSetting.Enabled = false;
                            gameSetting.SetGroupId(currencyGroup.Id.ToString());

                            var updateResult = await this.currencyGroupGameService.Update(gameSetting);

                            results.Add(currencyGroupName, new MessageResult(updateResult.Success, updateResult.Error));

                        }
                        catch (Exception ex)
                        {
                            sentryClient.CaptureException(ex);
                            results.Add(currencyGroupName, new MessageResult(false, ex.Message));
                        }
                    }
                }
            }

            return results; // Ensure a boolean value is returned
        }
    }
}