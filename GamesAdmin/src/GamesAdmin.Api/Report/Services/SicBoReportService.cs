using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database;

namespace GamesAdmin.Api.Report
{
    public class SicBoReportService : IGameReportService

    {
        private readonly ISicBoReportRepository repository;
        private readonly IMapper mapper;

        public SicBoReportService(ISicBoReportRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<BetReportBase> Get(BetReportFilter filter)
        {
            var result = await repository.GetAsync(filter);

            return new SicBoBetReport
            {
                RoundResult = mapper.Map<SicBoRoundResult>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<SicBoBetRecord>(x))
            };
        }
    }
}