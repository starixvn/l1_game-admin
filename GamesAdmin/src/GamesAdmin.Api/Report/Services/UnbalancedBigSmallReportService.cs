using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.Report
{
    public class UnbalancedBigSmallReportService<TUnbalancedBigSmallRound, TUnbalancedBigSmallBet, TUnbalancedBigSmallBetLog> : IGameReportService, IBetHistoryReportService
        where TUnbalancedBigSmallRound: UnbalancedBigSmallRoundEntity, new()
        where TUnbalancedBigSmallBet : UnbalancedBigSmallBetEntity
        where TUnbalancedBigSmallBetLog : UnbalancedBigSmallBetLogEntity
    {
        private readonly IUnbalancedBigSmallReportRepository repository;
        private readonly IMapper mapper;
        private readonly IBetHistoryReportRepository betHistoryRepository;

        public UnbalancedBigSmallReportService(IUnbalancedBigSmallReportRepository repository, IBetHistoryReportRepository betHistoryRepository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.betHistoryRepository = betHistoryRepository;
        }

        public async Task<BetReportBase> Get(BetReportFilter filter)
        {
            var result = await repository.GetAsync<TUnbalancedBigSmallRound, TUnbalancedBigSmallBet>(filter);

            return new UnbalancedBigSmallBetReport
            {
                RoundResult = mapper.Map<BigSmallRoundResult>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<UnbalancedBigSmallBetRecord>(x))
            };
        }

        public async Task<BetHistory> Get(BetHistoryFilter filter)
        {
            var result = await betHistoryRepository.GetUnbalancedBigSmallAsync<TUnbalancedBigSmallRound, TUnbalancedBigSmallBetLog>(filter.RoundId, filter.UserId, filter.PartnerId);

            return new UnbalancedBigSmallBetHistory
            {
                RoundResult = mapper.Map<BigSmallRoundResult>(result.Item1),
                Bets = result.Item2.Select(x => mapper.Map<UnbalancedBigSmallBetHistoryRecord>(x)),
            };
        }
    }
}
