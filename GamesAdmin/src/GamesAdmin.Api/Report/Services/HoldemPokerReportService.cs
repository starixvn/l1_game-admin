using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.Currencies;
using GamesAdmin.Api.CurrencyGroups;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Entities.Currencies;
using GamesAdmin.Database.Report.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace GamesAdmin.Api.Report
{
    public interface IHoldemPokerReportService
    {
        Task<IEnumerable<HoldemPokerBetRecord>> GetHoldemPokerReportAsync(HoldemPokerBetReportFilter filter);

        Task<HoldemPokerBetHistory> GetHoldemPokerBetHistoryAsync(HoldemPokerBetHistoryFilter filter);
    }

    public class HoldemPokerReportService : IHoldemPokerReportService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IMapper mapper;
        private readonly ICurrencyService currencyService;
        private readonly ICurrencyGroupService currencyGroupService;

        private readonly Dictionary<GameId, Type> repositories = new Dictionary<GameId, Type>
        {
            { GameId.CasinoHoldemPoker,  typeof(HoldemPokerReportRepository<CasinoHoldemPokerRoundEntity, CasinoHoldemPokerBetEntity, CasinoHoldemPokerBetReportEntity, CasinoHoldemPokerBetHistoryEntity>) },
            { GameId.TexasHoldemPoker,  typeof(HoldemPokerReportRepository<TexasHoldemPokerRoundEntity, TexasHoldemPokerBetEntity, TexasHoldemPokerBetReportEntity, TexasHoldemPokerBetHistoryEntity>) }
        };

        public HoldemPokerReportService(IServiceProvider serviceProvider,
            IMapper mapper,
            ICurrencyService currencyService,
            ICurrencyGroupService currencyGroupService)
        {
            this.serviceProvider = serviceProvider;
            this.mapper = mapper;
            this.currencyService = currencyService;
            this.currencyGroupService = currencyGroupService;
        }

        public async Task<IEnumerable<HoldemPokerBetRecord>> GetHoldemPokerReportAsync(HoldemPokerBetReportFilter filter)
        {
            var repository = (IHoldemPokerReportRepository)serviceProvider.GetRequiredService(repositories[filter.GameId]);
            var result = await repository.GetAsync(filter);

            return result.Select(x => mapper.Map<HoldemPokerBetRecord>(x));
        }

        public async Task<HoldemPokerBetHistory> GetHoldemPokerBetHistoryAsync(HoldemPokerBetHistoryFilter filter)
        {
            var repository = (IHoldemPokerReportRepository)serviceProvider.GetRequiredService(repositories[filter.GameId]);
            var betHistory = await repository.GetBetHistoryAsync(filter);

            var currency = (await currencyService.GetByPartner(betHistory.OperatorId)).FirstOrDefault(c => c.Name == betHistory.Currency);
            var currencyGroup = await currencyGroupService.GetById(currency.GroupId);
            var result = mapper.Map<HoldemPokerBetHistory>(betHistory);
            result.FractionalOddsFormat = currencyGroup.FractionalOddsFormat;
            return result;
        }
    }
}