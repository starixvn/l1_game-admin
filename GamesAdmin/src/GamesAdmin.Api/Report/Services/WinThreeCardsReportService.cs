using AutoMapper;
using GamesAdmin.Api.Currencies;
using GamesAdmin.Api.CurrencyGroups;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Report
{
    public class WinThreeCardsReportService : IGameReportService, IBetHistoryReportService
    {
        private readonly IWinThreeCardsReportRepository repository;
        private readonly IMapper mapper;
        private readonly ICurrencyService currencyService;
        private readonly ICurrencyGroupService currencyGroupService;

        public WinThreeCardsReportService(IWinThreeCardsReportRepository repository,
            IMapper mapper,
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

            return new WinThreeCardsBetReport
            {
                RoundResult = mapper.Map<WinThreeCardsRoundResult>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<WinThreeCardsBetRecord>(x))
            };
        }

        public async Task<BetHistory> Get(BetHistoryFilter filter)
        {
            var result = await repository.GetBetHistoryAsync(filter);

            var betHistory = new WinThreeCardsBetHistory
            {
                RoundResult = mapper.Map<WinThreeCardsRoundResult>(result.Item1),
                Bets = result.Item2.Select(bet => mapper.Map<WinThreeCardsBetHistoryRecord>(bet)),
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