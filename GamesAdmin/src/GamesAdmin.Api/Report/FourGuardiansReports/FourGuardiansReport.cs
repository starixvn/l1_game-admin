using GamesAdmin.Core.Models;
using GamesAdmin.Database;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;

namespace GamesAdmin.Api.Report
{
    public class FourGuardiansReport : IFourGuardiansReport
    {
        private readonly IFourGuardiansReportRepository repository;
        private readonly IMapper mapper;

        public FourGuardiansReport(IFourGuardiansReportRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<FourGuardiansBetRecord>> GetReportAsync(long handId, string nickname, bool excludeUUS)
        {
            var result = await repository.GetAsync(handId, nickname, excludeUUS);

            return result.Select(x => mapper.Map<FourGuardiansBetRecord>(x));
        }
    }
}
