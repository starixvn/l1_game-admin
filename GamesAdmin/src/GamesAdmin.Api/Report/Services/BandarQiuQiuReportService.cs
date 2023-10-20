using System.Linq;
using AutoMapper;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models;
using System.Threading.Tasks;
using GamesAdmin.Database.Report;
using GamesAdmin.Api.Currencies;
using GamesAdmin.Api.CurrencyGroups;

namespace GamesAdmin.Api.Report
{
    public class BandarQiuQiuReportService : IGameReportService, IBetHistoryReportService
    {
        private readonly IBandarQiuQiuReportRepository repository;
        private readonly IMapper mapper;
        private readonly ICurrencyService currencyService;
        private readonly ICurrencyGroupService currencyGroupService;

        public BandarQiuQiuReportService(IBandarQiuQiuReportRepository repository, IMapper mapper,
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

            return new BandarQiuQiuBetReport
            {
                RoundResult = mapper.Map<BandarQiuQiuRoundResult>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<BandarQiuQiuBetRecord>(x))
            };
        }

        public async Task<BetHistory> Get(BetHistoryFilter filter)
        {
            var result = await repository.GetBetHistoryAsync(filter);

            var betHistory = new BandarQiuQiuBetHistory
            {
                RoundResult = mapper.Map<BandarQiuQiuRoundResult>(result.Item1),
                Bets = result.Item2.Select(x => mapper.Map<BandarQiuQiuBetHistoryRecord>(x)),
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