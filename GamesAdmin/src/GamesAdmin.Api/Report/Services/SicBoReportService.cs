using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database;

namespace GamesAdmin.Api.Report
{
    public class SicBoReportService : IGameReportService, IBetHistoryReportService

    {
        private readonly ISicBoReportRepository repository;
        private readonly IMapper mapper;
        private readonly IBetHistoryReportRepository betHistoryRepository;

        public SicBoReportService(ISicBoReportRepository repository, IBetHistoryReportRepository betHistoryRepository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.betHistoryRepository = betHistoryRepository;
        }

        public async Task<BetReportBase> Get(BetReportFilter filter)
        {
            var result = await repository.GetAsync(filter);

            return new SicBoBetReport
            {
                RoundResult = mapper.Map<SicBoRoundResult>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<SicBoBetRecord>(x))
            };
        }

        public async Task<BetHistory> Get(BetHistoryFilter filter)
        {
            var result = await betHistoryRepository.GetSicBoAsync(filter.RoundId, filter.UserId, filter.PartnerId);

            return new SicBoBetHistory
            {
                RoundResult = mapper.Map<SicBoRoundResult>(result.Item1),
                Bets = result.Item2.Select(x => mapper.Map<SicBoBetHistoryRecord>(x))
            };
        }
    }
}