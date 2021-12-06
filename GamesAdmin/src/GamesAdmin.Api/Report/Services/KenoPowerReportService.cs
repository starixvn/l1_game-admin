using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.Report
{
    public class KenoPowerReportService<TKenoPowerRound, TKenoPowerBet> : IGameReportService
        where TKenoPowerRound: KenoPowerRoundEntity, new()
        where TKenoPowerBet : KenoPowerOneEntity
    {
        private readonly IKenoPowerReportRepository repository;
        private readonly IMapper mapper;

        public KenoPowerReportService(IKenoPowerReportRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<BetReportBase> Get(BetReportFilter filter)
        {
            var result = await repository.GetAsync<TKenoPowerRound, TKenoPowerBet>(filter);

            return new KenoPowerBetReport
            {
                RoundResult = mapper.Map<KenoPowerRoundResult>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<KenoPowerBetRecord>(x))
            };
        }
    }
}
