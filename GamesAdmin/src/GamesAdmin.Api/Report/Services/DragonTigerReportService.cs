using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database;

namespace GamesAdmin.Api.Report
{
    public class DragonTigerReportService : IGameReportService

    {
        private readonly IDragonTigerReportRepository repository;
        private readonly IMapper mapper;

        public DragonTigerReportService(IDragonTigerReportRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<BetReportBase> Get(BetReportFilter filter)
        {
            var result = await repository.GetAsync(filter);

            return new DragonTigerBetReport
            {
                RoundResult = mapper.Map<DragonTigerRoundResult>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<DragonTigerBetRecord>(x))
            };
        }
    }
}