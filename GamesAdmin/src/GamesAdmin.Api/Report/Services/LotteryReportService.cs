using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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

        public LotteryReportService(ILotteryReportRepository repository, IBetHistoryReportRepository betHistoryRepository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.betHistoryRepository = betHistoryRepository;
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

            return new LotteryBetHistory
            {
                RoundResult = mapper.Map<LotteryRoundResult>(result.Item1),
                Bets = result.Item2.Select(x => mapper.Map<LotteryBetHistoryRecord>(x)),
                Odds = mapper.Map<LotteryOdds>(result.Item3)
            };
        }
    }
}
