using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.Report
{
    public class MiniSeDieMultiplayReportService<TRoundEntity> : IGameReportService
        where TRoundEntity : MiniSeDieMultiplayRoundEntity, new()
    {
        private readonly IMiniSeDieMultiplayReportRepository repository;
        private readonly IMapper mapper;

        public MiniSeDieMultiplayReportService(IMiniSeDieMultiplayReportRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<BetReportBase> Get(BetReportFilter filter)
        {
            var result = await repository.GetAsync<TRoundEntity>(filter);

            return new SeDieBetReport
            {
                RoundResult = mapper.Map<SeDieRoundResult>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<SeDieBetRecord>(x))
            };
        }
    }
}