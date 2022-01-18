using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Database;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<SlotGameBetRecord>> GetReportAsync(long handId, string nickname, bool excludeUUS)
        {
            var result = await repository.GetAsync(handId, nickname, excludeUUS);

            return result.Select(x => mapper.Map<SlotGameBetRecord>(x));
        }
    }
}
