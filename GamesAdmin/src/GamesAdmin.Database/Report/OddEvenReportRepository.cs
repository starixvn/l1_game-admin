using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IOddEvenReportRepository
    {
        Task<(OddEvenRoundEntity, IEnumerable<OddEvenBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeBot);
    }

    public class OddEvenReportRepository : BaseBetReportRepository, IOddEvenReportRepository
    {
        public OddEvenReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(OddEvenRoundEntity, IEnumerable<OddEvenBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeBot)
        {
            var roundResult = await database.GetCollection<OddEvenRoundEntity>(Document.GetCollectionName(typeof(OddEvenRoundEntity))).Find(x => x.Number == roundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var records = await GetBaseAggregateBuilder<OddEvenBetEntity, OddEvenBetWithUnwindAccountsEntity>(roundId, nickname, excludeBot)
                    .Project(x => new OddEvenBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        Nickname = x.AccountInfos.Name,
                        Select = x.Select,
                        Bet = x.Bet,
                        BetWin = x.Betwin,
                        Back = x.Back
                    }).ToListAsync();

                return (roundResult, records);
            }

            return (new OddEvenRoundEntity(), Enumerable.Empty<OddEvenBetReportEntity>());
        }
    }
}