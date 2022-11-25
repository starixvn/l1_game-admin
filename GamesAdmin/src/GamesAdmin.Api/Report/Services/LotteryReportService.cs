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
    public class LotteryReportService : IGameReportService, IBetHistoryReportService

    {
        private readonly ILotteryReportRepository repository;
        private readonly IMapper mapper;
        private readonly IBetHistoryReportRepository betHistoryRepository;
        private readonly ICurrencyService currencyService;
        private readonly ICurrencyGroupService currencyGroupService;

        public LotteryReportService(ILotteryReportRepository repository,
            IBetHistoryReportRepository betHistoryRepository,
            IMapper mapper,
            ICurrencyService currencyService,
            ICurrencyGroupService currencyGroupService)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.betHistoryRepository = betHistoryRepository;
            this.currencyService = currencyService;
            this.currencyGroupService = currencyGroupService;
        }

        public async Task<BetReportBase> Get(BetReportFilter filter)
        {
            var result = await repository.GetAsync(filter);

            return new LotteryBetReport
            {
                RoundResult = mapper.Map<LotteryRoundResult>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<LotteryBetRecord>(x))
            };
        }

        public async Task<BetHistory> Get(BetHistoryFilter filter)
        {
            var result = await betHistoryRepository.GetLotteryAsync(filter.RoundId, filter.UserId, filter.PartnerId);
            var betHistory = new LotteryBetHistory
            {
                RoundResult = mapper.Map<LotteryRoundResult>(result.Item1),
                Bets = result.Item2.Select(x => mapper.Map<LotteryBetHistoryRecord>(x)),
                Odds = mapper.Map<LotteryOdds>(result.Item3)
            };
            if (result.Item2.Any())
            {
                var bet = result.Item2.First();
                var currency = (await currencyService.GetByPartner(bet.OperatorId)).FirstOrDefault(c => c.Name == bet.Currency);
                var currencyGroup = await currencyGroupService.GetById(currency.GroupId);
                betHistory.FractionalOddsFormat = currencyGroup.FractionalOddsFormat;
            };

            return betHistory;
        }
    }
}