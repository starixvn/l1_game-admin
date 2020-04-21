using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IBigSmallTurboReportRepository
    {
        Task<(BigSmallTurboRoundEntity, IEnumerable<BetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeBot);
    }

    public class BigSmallTurboReportRepository : BaseBetReportRepository, IBigSmallTurboReportRepository
    {
        public BigSmallTurboReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(BigSmallTurboRoundEntity, IEnumerable<BetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeBot)
        {
            return await GetAsync<BigSmallTurboRoundEntity, BigSmallTurboBetEntity>(roundId, nickname, excludeBot);
        }
    }
}