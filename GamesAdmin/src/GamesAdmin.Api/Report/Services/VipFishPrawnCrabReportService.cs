using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database;

namespace GamesAdmin.Api.Report
{
    public class VipFishPrawnCrabReportService : IGameReportService

    {
        private readonly IVipFishPrawnCrabReportRepository repository;
        private readonly IMapper mapper;

        public VipFishPrawnCrabReportService(IVipFishPrawnCrabReportRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<BetReportBase> Get(BetReportFilter filter)
        {
            var result = await repository.GetAsync(filter);

            return new VipFishPrawnCrabBetReport
            {
                RoundResult = mapper.Map<VipFishPrawnCrabRoundResult>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<VipFishPrawnCrabBetRecord>(x))
            };
        }
    }
}