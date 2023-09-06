using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database;

namespace GamesAdmin.Api.Report
{
    public class BlackjackReportService
    {
        private readonly IBlackjackReportRepository repository;
        private readonly IMapper mapper;

        public BlackjackReportService(IBlackjackReportRepository repository, IMapper mapper)
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