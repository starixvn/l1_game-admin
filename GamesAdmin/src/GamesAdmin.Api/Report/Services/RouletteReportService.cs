using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database;
namespace GamesAdmin.Api.Report
{
    public class RouletteReportService : IGameReportService

    {
        private readonly IRouletteReportRepository repository;
        private readonly IMapper mapper;

        public RouletteReportService(IRouletteReportRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
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
    }
}
