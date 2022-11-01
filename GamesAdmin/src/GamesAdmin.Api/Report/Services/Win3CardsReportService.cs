using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Report.Services
{
    public class Win3CardsReportService : IGameReportService
    {
        private readonly IWin3CardsReportRepository repository;
        private readonly IMapper mapper;

        public Win3CardsReportService(IWin3CardsReportRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<BetReportBase> Get(BetReportFilter filter)
        {
            var result = await repository.GetAsync(filter);

            return new Win3CardsBetReport
            {
                RoundResult = mapper.Map<Win3CardsRoundResult>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<Win3CardsBetRecord>(x))
            };
        }
    }
}
