using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database;

namespace GamesAdmin.Api.Report
{
    public class HiLoReportService
    {
        private readonly IHiLoReportRepository repository;
        private readonly IMapper mapper;

        public HiLoReportService(IHiLoReportRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<HiLoBetRecord>> Get(HiLoBetReportFilter filter)
        {
            var result = await repository.GetAsync(filter);

            return result.Select(x => mapper.Map<HiLoBetRecord>(x));
        }
    }
}