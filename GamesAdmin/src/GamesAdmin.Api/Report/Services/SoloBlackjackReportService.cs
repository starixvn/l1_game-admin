using AutoMapper;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models;
using GamesAdmin.Database;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using GamesAdmin.Database.Report;

namespace GamesAdmin.Api.Report.Services
{
    public class SoloBlackjackReportService
    {
        private readonly ISoloBlackjackReportRepository repository;
        private readonly IMapper mapper;

        public SoloBlackjackReportService(ISoloBlackjackReportRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<BlackjackBetRecord>> Get(BlackjackBetReportFilter filter)
        {
            var result = await repository.GetAsync(filter);

            return result.Select(x => mapper.Map<BlackjackBetRecord>(x));
        }
    }
}
