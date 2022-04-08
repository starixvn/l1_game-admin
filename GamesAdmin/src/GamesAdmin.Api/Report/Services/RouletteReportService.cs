using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database;
namespace GamesAdmin.Api.Report
{
    public class RouletteReportService : IGameReportService, IBetHistoryReportService

    {
        private readonly IRouletteReportRepository repository;
        private readonly IMapper mapper;
        private readonly IBetHistoryReportRepository betHistoryRepository;
        public RouletteReportService(IRouletteReportRepository repository, IBetHistoryReportRepository betHistoryRepository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.betHistoryRepository = betHistoryRepository;
        }

        public async Task<BetReportBase> Get(BetReportFilter filter)
        {
            var result = await repository.GetAsync(filter);

            return new RouletteBetReport
            {
                RoundResult = mapper.Map<RouletteRoundResult>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<RouletteBetRecord>(x))
            };
        }

        public async Task<BetHistory> Get(BetHistoryFilter filter)
        {
            var result = await betHistoryRepository.GetRouletteAsync(filter.RoundId, filter.UserId, filter.PartnerId);

            return new RouletteBetHistory
            {
                RoundResult = mapper.Map<RouletteRoundResult>(result.Item1),
                Bets = result.Item2.Select(x => mapper.Map<RouletteBetHistoryRecord>(x))
            };
        }
    }
}
