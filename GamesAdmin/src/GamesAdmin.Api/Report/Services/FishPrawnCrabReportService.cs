using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database;

namespace GamesAdmin.Api.Report
{
    public class FishPrawnCrabReportService : IGameReportService

    {
        private readonly IFishPrawnCrabReportRepository repository;
        private readonly IMapper mapper;

        public FishPrawnCrabReportService(IFishPrawnCrabReportRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<BetReportBase> Get(BetReportFilter filter)
        {
            var result = await repository.GetAsync(filter);

            return new FishPrawnCrabBetReport
            {
                RoundResult = mapper.Map<FishPrawnCrabRoundResult>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<FishPrawnCrabBetRecord>(x))
            };
        }
    }
}