using AutoMapper;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Database.Report;
using System.Linq;

namespace GamesAdmin.Api.Report.Services
{
    public class PontoonReportService
    {
        private readonly IPontoonReportRepository repository;
        private readonly IMapper mapper;

        public PontoonReportService(IPontoonReportRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<BankingGameBetRecord>> Get(BankingGameBetReportFilter filter)
        {
            var result = await repository.GetAsync(filter);

            return result.Select(x => mapper.Map<BankingGameBetRecord>(x));
        }
    }
}
