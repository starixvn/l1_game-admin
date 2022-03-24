using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Report
{
    public class NightClubReport : ISlotGameReport
    {
        private readonly INightClubReportRepository repository;
        private readonly IMapper mapper;

        public NightClubReport(INightClubReportRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<SlotGameBetRecord>> GetReportAsync(SlotGameBetReportFilter filter)
        {
            var result = await repository.GetAsync(filter);

            return result.Select(x => mapper.Map<SlotGameBetRecord>(x));
        }
    }
}
