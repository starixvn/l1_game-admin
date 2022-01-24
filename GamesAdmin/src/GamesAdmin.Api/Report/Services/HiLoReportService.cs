using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database;

namespace GamesAdmin.Api.Report
{
    public class HiLoReportService : IBetHistoryReportService
    {
        private readonly IHiLoReportRepository repository;
        private readonly IMapper mapper;
        private readonly IBetHistoryReportRepository betHistoryRepository;

        public HiLoReportService(IHiLoReportRepository repository, IBetHistoryReportRepository betHistoryRepository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.betHistoryRepository = betHistoryRepository;
        }

        public async Task<IEnumerable<HiLoBetRecord>> Get(HiLoBetReportFilter filter)
        {
            var result = await repository.GetAsync(filter);

            return result.Select(x => mapper.Map<HiLoBetRecord>(x));
        }

        public async Task<BetHistory> Get(BetHistoryFilter filter)
        {
            var result = await betHistoryRepository.GetHiLoBetAsync(filter.RoundId, filter.UserId, filter.PartnerId);

            return new HiLoBetHistory
            {
                RoundResult = mapper.Map<HiLoRoundResult>(result.Item1),
                Bets = result.Item2.Select(x => mapper.Map<HiLoBetHistoryRecord>(x))
            };
        }
    }
}