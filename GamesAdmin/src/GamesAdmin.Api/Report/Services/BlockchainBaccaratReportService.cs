using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database;
namespace GamesAdmin.Api.Report
{
    public class BlockchainBaccaratReportService : IGameReportService
    {
        private readonly IBlockchainBaccaratReportRepository repository;
        private readonly IMapper mapper;

        public BlockchainBaccaratReportService(IBlockchainBaccaratReportRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<BetReportBase> Get(BetReportFilter filter)
        {
            try
            {
                var result = await repository.GetAsync(filter);

                return new BlockchainBaccaratBetReport
                {
                    RoundResult = mapper.Map<BaccaratRoundResult>(result.Item1),
                    Records = result.Item2.Select(x => mapper.Map<BaccaratBetRecord>(x))
                };
            }
            catch (Exception ex)
            {
                var aa = ex;
                return null;
            }

        }
    }
}
