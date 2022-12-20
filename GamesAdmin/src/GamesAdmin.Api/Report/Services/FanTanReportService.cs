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
    public class FanTanReportService : IGameReportService, IBetHistoryReportService
    {
        private readonly IFanTanReportRepository repository;
        private readonly IMapper mapper;
        private readonly IBetHistoryReportRepository betHistoryRepository;
        private readonly ICurrencyService currencyService;
        private readonly ICurrencyGroupService currencyGroupService;

        public FanTanReportService(
            IFanTanReportRepository repository,
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

            return new FanTanBetReport
            {
                RoundResult = mapper.Map<FanTanRoundResult>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<FanTanBetRecord>(x))
            };
        }

        public async Task<BetHistory> Get(BetHistoryFilter filter)
        {
            var result = await betHistoryRepository.GetFanTanAsync(filter.RoundId, filter.UserId, filter.PartnerId);

            var betHistory = new FanTanBetHistory
            {
                RoundResult = mapper.Map<FanTanRoundResult>(result.Item1),
                Bet = mapper.Map<FanTanBetHistoryRecord>(result.Item2)
            };
            if (result.Item2 != null)
            {
                var currency = (await currencyService.GetByPartner(result.Item2.OperatorId)).FirstOrDefault(c => c.Name == result.Item2.Currency);
                var currencyGroup = await currencyGroupService.GetById(currency.GroupId);
                betHistory.FractionalOddsFormat = currencyGroup.FractionalOddsFormat;
            }
            return betHistory;
        }
    }
}