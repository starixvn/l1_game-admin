using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.Report
{
    public class KenoMatchReportService<TKenoMatchRound, TKenoMatchBet, TKenoMatchBetLog> : IGameReportService, IBetHistoryReportService
        where TKenoMatchRound: KenoMatchRoundEntity, new()
        where TKenoMatchBet : KenoMatchOneEntity
        where TKenoMatchBetLog : KenoMatchBetEntity
    {
        private readonly IKenoMatchReportRepository repository;
        private readonly IMapper mapper;
        private readonly IBetHistoryReportRepository betHistoryRepository;

        public KenoMatchReportService(IKenoMatchReportRepository repository, IBetHistoryReportRepository betHistoryRepository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.betHistoryRepository = betHistoryRepository;
        }

        public async Task<BetReportBase> Get(BetReportFilter filter)
        {
            var result = await repository.GetAsync<TKenoMatchRound, TKenoMatchBet>(filter);

            return new KenoMatchBetReport
            {
                RoundResult = mapper.Map<KenoMatchRoundResult>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<KenoMatchBetRecord>(x))
            };
        }

        public async Task<BetHistory> Get(BetHistoryFilter filter)
        {
            var result = await betHistoryRepository.GetKenoMatchAsync<TKenoMatchRound, TKenoMatchBetLog>(filter.RoundId, filter.UserId, filter.PartnerId);

            return new KenoMatchBetHistory
            {
                RoundResult = mapper.Map<KenoMatchRoundResult>(result.Item1),
                Bets = result.Item2.Select(x => mapper.Map<KenoMatchBetHistoryRecord>(x)),
            };
        }
    }
}
