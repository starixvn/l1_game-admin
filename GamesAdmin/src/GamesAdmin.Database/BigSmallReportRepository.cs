using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IBigSmallReportRepository
    {
        Task<(BigSmallRoundEntity, IEnumerable<BetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeBot);
    }

    public class BigSmallReportRepository : BaseBetReportRepository, IBigSmallReportRepository
    {
        public BigSmallReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(BigSmallRoundEntity, IEnumerable<BetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeBot)
        {
            return await GetAsync<BigSmallRoundEntity, BigSmallBetEntity>(roundId, nickname, excludeBot);
        }
    }
}