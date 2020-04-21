using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IOddEvenTurboReportRepository
    {
        Task<(OddEvenTurboRoundEntity, IEnumerable<BetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeBot);
    }

    public class OddEvenTurboReportRepository : BaseBetReportRepository, IOddEvenTurboReportRepository
    {
        public OddEvenTurboReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(OddEvenTurboRoundEntity, IEnumerable<BetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeBot)
        {
            return await GetAsync<OddEvenTurboRoundEntity, OddEvenTurboBetEntity>(roundId, nickname, excludeBot);
        }
    }
}