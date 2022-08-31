using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Report;

namespace GamesAdmin.Api.Report
{
    public class FruitSlotReportService : IGameReportService, IBetHistoryReportService
    {
        private readonly IFruitSlotReportRepository repository;
        private readonly IMapper mapper;

        public FruitSlotReportService(IFruitSlotReportRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<BetReportBase> Get(BetReportFilter filter)
        {
            var result = await repository.GetRecordAsync(filter);

            return new FruitSlotBetReport
            {
                RoundResult = mapper.Map<FruitSlotRoundResult>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<FruitSlotBetRecord>(x))
            };
        }

        public async Task<BetHistory> Get(BetHistoryFilter filter)
        {
            var result = await repository.GetBetHistoryAsync(filter);

            return new FruitSlotBetHistory
            {
                RoundResult = mapper.Map<FruitSlotRoundResult>(result.Item1),
                Bets = result.Item2.Select(x => mapper.Map<FruitSlotBetHistoryRecord>(x)),
                Odds = result.Item3
            };
        }
    }
}