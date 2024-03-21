using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.Currencies;
using GamesAdmin.Api.CurrencyGroups;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database;

namespace GamesAdmin.Api.Report
{
    public class FootballDiceReportService : IGameReportService, IBetHistoryReportService

    {
        private readonly IFootballDiceReportRepository repository;
        private readonly IMapper mapper;
        private readonly ICurrencyService currencyService;
        private readonly ICurrencyGroupService currencyGroupService;


        public FootballDiceReportService(IFootballDiceReportRepository repository, IMapper mapper,
            ICurrencyService currencyService,
            ICurrencyGroupService currencyGroupService)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.currencyService = currencyService;
            this.currencyGroupService = currencyGroupService;
        }

        public async Task<BetReportBase> Get(BetReportFilter filter)
        {
            var result = await repository.GetRecordAsync(filter);

            return new FootballDiceBetReport
            {
                RoundResult = mapper.Map<FootballDiceRoundResult>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<FootballDiceBetRecord>(x))
            };
        }

        public async Task<BetHistory> Get(BetHistoryFilter filter)
        {
            var result = await repository.GetBetHistoryAsync(filter);

            var betHistory = new FootballDiceBetHistory
            {
                RoundResult = mapper.Map<FootballDiceRoundResult>(result.Item1),
                Bets = result.Item2.Select(x => mapper.Map<FootballDiceBetHistoryRecord>(x)),
                Odds = result.Item3
            };
            if (result.Item2.Any())
            {
                var currency = (await currencyService.GetByPartner(filter.PartnerId)).FirstOrDefault(c => c.Name == result.Item2.FirstOrDefault().Currency);
                var currencyGroup = await currencyGroupService.GetById(currency.GroupId);
                betHistory.FractionalOddsFormat = currencyGroup.FractionalOddsFormat;
            }
            return betHistory;
        }
    }
}