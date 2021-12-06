using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.Report
{
    public class KenoMatchReportService<TKenoMatchRound, TKenoMatchBet> : IGameReportService
        where TKenoMatchRound: KenoMatchRoundEntity, new()
        where TKenoMatchBet : KenoMatchOneEntity
    {
        private readonly IKenoMatchReportRepository repository;
        private readonly IMapper mapper;

        public KenoMatchReportService(IKenoMatchReportRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
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
    }
}
