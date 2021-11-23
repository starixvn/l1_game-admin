using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Report
{
    public class HeroesSlotReport : ISlotGameReport
    {
        private readonly IHeroesSlotReportRepository repository;
        private readonly IMapper mapper;

        public HeroesSlotReport(IHeroesSlotReportRepository repository, IMapper mapper)
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
