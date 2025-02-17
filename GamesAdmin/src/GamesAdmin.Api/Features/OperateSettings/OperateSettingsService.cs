using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api.Currencies;
using GamesAdmin.Api.CurrencyGroupGames;
using GamesAdmin.Api.CurrencyGroups;
using GamesAdmin.Api.Features.OperateSettings.Models;
using GamesAdmin.Api.Partners;
using System.Text.Json;
using GamesAdmin.Api.Games;
using System.Linq;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database;
using GamesAdmin.Core.Models;
using Sentry;
using GamesAdmin.Core.Models.Currencies;
using CurrencyHandlers = GamesAdmin.Api.Currencies.Handlers;
using CurrencyGroupHandlers = GamesAdmin.Api.CurrencyGroups.Handlers;
using GamesAdmin.Database.Entities.Currencies;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MediatR;
using CurrenciesRequests = GamesAdmin.Api.Currencies.Requests;
using CurrencyGroupRequests = GamesAdmin.Api.CurrencyGroups.Requests;
using AutoMapper;
using GamesAdmin.Api.Currencies.Requests;
using GamesAdmin.Api.CurrencyGroups.Requests;

namespace GamesAdmin.Api.Features.OperateSettings
{
    public interface IOperateSettingsService
    {
        Task<OperateGameSettings> GetOperateGameSettingsAsync(IList<int> gameIds);
    }

    public class OperateSettingsService : IOperateSettingsService
    {
        private readonly ICurrencyService currencyService;
        private readonly ICurrencyGroupService currencyGroupService;
        private readonly IGameSettingsService gameSettingsService;
        private readonly ISentryClient sentryClient;
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public OperateSettingsService(
            ICurrencyService currencyService,
            ICurrencyGroupService currencyGroupService,
            IGameSettingsService gameSettingsService,
            ISentryClient sentryClient,
            IMediator mediator,
            IMapper mapper)
        {
            this.currencyService = currencyService;
            this.gameSettingsService = gameSettingsService;
            this.sentryClient = sentryClient;

            this.currencyGroupService = currencyGroupService;
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public async Task<OperateGameSettings> GetOperateGameSettingsAsync(IList<int> gameIds)
        {
            var allGameSettings = await gameSettingsService.Get();
            var filterGames = gameIds != null && gameIds.Any() ? allGameSettings.Where(gs => gameIds.Contains(gs.Info.GameId)) : allGameSettings;

            return new OperateGameSettings
            {
                GameConfigs = filterGames.ToList(),
                GameConfigJson = JsonSerializer.Serialize(filterGames)
            };
        }

        public async Task<bool> ImportGameSetting(IList<GameConfig> gameConfigs)
        {
            try
            {
                foreach (var gameConfig in gameConfigs)
                {
                    var dbGameConfig = await gameSettingsService.GetByName(gameConfig.Info.Name);

                    if (dbGameConfig == null)
                    {
                        await gameSettingsService.Create(gameConfig);
                    }

                    await gameSettingsService.UpdateOdds(gameConfig.Name, gameConfig.Odds);
                    await gameSettingsService.UpdateMarginSettings(gameConfig.Name, gameConfig.Margin);

                    return true;
                }
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return false;
        }

        public async Task<bool> ImportCurrency(IList<CurrencyModel> currencies)
        {
            try
            {
                foreach (var currency in currencies)
                {
                    var dbCurrency = await currencyService.GetById(currency.Id);

                    if (dbCurrency == null)
                    {
                        await mediator.Send(new CurrenciesRequests.CreateRequest(this.mapper.Map<CreateCurrencyModel>(currency)));
                    }
                }
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return false;
        }

        public async Task<bool> ImportCurrencyGroup(IList<CurrencyGroupModel> currencyGroups)
        {
            try
            {
                var allDBCurrencyGroups = await currencyGroupService.GetAll();
                foreach (var currencyGroup in currencyGroups)
                {
                    if (!allDBCurrencyGroups.Any(dbCurrencyGroup => 
                        dbCurrencyGroup.Name.ToLower() == currencyGroup.Name.ToLower()
                        && dbCurrencyGroup.PartnerId.ToLower() == currencyGroup.PartnerId.ToLower()))
                    {
                        await mediator.Send(new CurrencyGroupRequests.CreateRequest(this.mapper.Map<CreateCurrencyGroupModel>(currencyGroup)));
                    }
                }
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return false;
        }
    }
}