using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database;
namespace GamesAdmin.Api.Report
{
    public class LotteryReportService : IGameReportService

    {
        private readonly ILotteryReportRepository repository;
        private readonly IMapper mapper;

        public LotteryReportService(ILotteryReportRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<BetReportBase> Get(BetReportFilter filter)
        {
            var result = await repository.GetAsync(filter);

            return new LotteryBetReport
            {
                RoundResult = mapper.Map<LotteryRoundResult>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<LotteryBetRecord>(x))
            };
        }
    }
}
