using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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

        public FanTanReportService(
            IFanTanReportRepository repository,
            IBetHistoryReportRepository betHistoryRepository,
            IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.betHistoryRepository = betHistoryRepository;
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

            return new FanTanBetHistory
            {
                RoundResult = mapper.Map<FanTanRoundResult>(result.Item1),
                Bet = mapper.Map<FanTanBetHistoryRecord>(result.Item2)
            };
        }
    }
}
