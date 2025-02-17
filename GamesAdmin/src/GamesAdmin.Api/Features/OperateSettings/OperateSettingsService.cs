using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.Currencies;
using GamesAdmin.Api.Currencies.Requests;
using GamesAdmin.Api.CurrencyGroups;
using GamesAdmin.Api.CurrencyGroups.Requests;
using GamesAdmin.Api.Features.OperateSettings.Models;
using GamesAdmin.Api.Games;
using GamesAdmin.Core.Models.Currencies;
using MediatR;
using Sentry;
using CurrenciesRequests = GamesAdmin.Api.Currencies.Requests;
using CurrencyGroupRequests = GamesAdmin.Api.CurrencyGroups.Requests;

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