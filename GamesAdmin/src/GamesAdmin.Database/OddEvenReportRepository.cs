using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IOddEvenReportRepository
    {
        Task<(OddEvenRoundEntity, IEnumerable<BetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeBot);
    }

    public class OddEvenReportRepository : BaseBetReportRepository, IOddEvenReportRepository
    {
        public OddEvenReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(OddEvenRoundEntity, IEnumerable<BetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeBot)
        {
            return await GetAsync<OddEvenRoundEntity, OddEvenBetEntity>(roundId, nickname, excludeBot);
        }
    }
}