using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database;

namespace GamesAdmin.Api.Report
{
    public class TreasureIslandReportService : ICascadingSlotReportService
    {
        private readonly ITreasureIslandReportRepository repository;
        private readonly IMapper mapper;

        public TreasureIslandReportService(ITreasureIslandReportRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<CascadingSlotBetRecord>> GetReportAsync(CascadingSlotReportFilter filter)
        {
            var result = await repository.GetAsync(filter);

            return result.Select(x => mapper.Map<CascadingSlotBetRecord>(x));
        }
    }
}
