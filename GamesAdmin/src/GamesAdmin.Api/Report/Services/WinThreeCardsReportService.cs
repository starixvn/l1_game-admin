using AutoMapper;
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

        public WinThreeCardsReportService(IWinThreeCardsReportRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
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

            return new WinThreeCardsBetHistory
            {
                RoundResult = mapper.Map<WinThreeCardsRoundResult>(result.Item1),
                Bet = mapper.Map<WinThreeCardsBetRecord>(result.Item2),
                Odds = result.Item3
            };
        }
    }
}