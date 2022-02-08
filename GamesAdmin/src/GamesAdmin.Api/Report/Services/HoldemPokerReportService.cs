using AutoMapper;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Report
{
    public interface IHoldemPokerReportService
    {
        Task<IEnumerable<HoldemPokerBetRecord>> GetHoldemPokerReportAsync(HoldemPokerBetReportFilter filter);

        Task<HoldemPokerBetHistory> GetHoldemPokerBetHistoryAsync(HoldemPokerBetHistoryFilter filter);
    }

    public class HoldemPokerReportService : IHoldemPokerReportService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IMapper mapper;
        private readonly Dictionary<GameId, Type> repositories = new Dictionary<GameId, Type>
        {
            { GameId.CasinoHoldemPoker,  typeof(CasinoHoldemPokerReportRepository) },
            { GameId.TexasHoldemPoker,  typeof(TexasHoldemPokerReportRepository)},
        };

        public HoldemPokerReportService(IServiceProvider serviceProvider, IMapper mapper)
        {
            this.serviceProvider = serviceProvider;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<HoldemPokerBetRecord>> GetHoldemPokerReportAsync(HoldemPokerBetReportFilter filter)
        {
            var repository = (IHoldemPokerReportRepository)serviceProvider.GetRequiredService(repositories[filter.GameId]);
            var result = await repository.GetAsync(filter);

            return result.Select(x => mapper.Map<HoldemPokerBetRecord>(x));
        }

        public async Task<HoldemPokerBetHistory> GetHoldemPokerBetHistoryAsync(HoldemPokerBetHistoryFilter filter)
        {
            var repository = (IHoldemPokerReportRepository)serviceProvider.GetRequiredService(repositories[filter.GameId]);
            var result = await repository.GetBetHistoryAsync(filter);

            return mapper.Map<HoldemPokerBetHistory>(result);
        }
    }
}
