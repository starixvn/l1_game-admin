using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Entities.Baccarat;
using Microsoft.Extensions.DependencyInjection;

namespace GamesAdmin.Api.Report
{
    public class BaccaratReportService : IGameReportService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IMapper mapper;
        private readonly Dictionary<int, Type> repositories = new Dictionary<int, Type>
        {
            { (int)GameId.Baccarat,  typeof(BaccaratReportRepository<BaccaratRoundEntity, BaccaratBetEntity>) },
            { (int)GameId.VipBaccarat,  typeof(BaccaratReportRepository<VipBaccaratRoundEntity, VipBaccaratBetEntity>) },
            { (int)GameId.BlockchainBaccarat,  typeof(BaccaratReportRepository<BlockchainBaccaratRoundEntity, BlockchainBaccaratBetEntity>) }
        };

        public BaccaratReportService(IServiceProvider serviceProvider, IMapper mapper)
        {
            this.serviceProvider = serviceProvider;
            this.mapper = mapper;
        }

        public async Task<BetReportBase> Get(BetReportFilter filter)
        {
            var repository = (IBaccaratBaseReportRepository)serviceProvider.GetRequiredService(repositories[filter.Game]);
            var result = await repository.GetAsync(filter);

            return new BaccaratBetReport
            {
                RoundResult = mapper.Map<BaccaratRoundResult>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<BaccaratBetRecord>(x))
            };
        }
    }
}
