using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database;

namespace GamesAdmin.Api.Report
{
    public class VipSeDieWarReportService : IGameReportService, IBetHistoryReportService

    {
        private readonly IVipSeDieWarReportRepository repository;
        private readonly IMapper mapper;
        private readonly IBetHistoryReportRepository betHistoryRepository;

        public VipSeDieWarReportService(IVipSeDieWarReportRepository repository, IBetHistoryReportRepository betHistoryRepository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<BetReportBase> Get(BetReportFilter filter)
        {
            var result = await repository.GetAsync(filter);

            return new VipSeDieWarBetReport
            {
                RoundResult = mapper.Map<VipSeDieWarRoundResult>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<VipSeDieWarBetRecord>(x))
            };
        }

        public async Task<BetHistory> Get(BetHistoryFilter filter)
        {
            var result = await betHistoryRepository.GetVipSeDieWarAsync(filter.RoundId, filter.UserId, filter.PartnerId);

            return new VipSeDieWarBetHistory
            {
                RoundResult = mapper.Map<VipSeDieWarRoundResult>(result.Item1),
                Bets = result.Item2.Select(x => mapper.Map<VipSeDieWarBetHistoryRecord>(x))
            };
        }
    }
}