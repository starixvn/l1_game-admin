using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IOddEvenTurboReportRepository
    {
        Task<(OddEvenTurboRoundEntity, IEnumerable<OddEvenBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeBot);
    }

    public class OddEvenTurboReportRepository : BaseBetReportRepository, IOddEvenTurboReportRepository
    {
        public OddEvenTurboReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(OddEvenTurboRoundEntity, IEnumerable<OddEvenBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeBot)
        {
            var roundResult = await database.GetCollection<OddEvenTurboRoundEntity>(Document.GetCollectionName(typeof(OddEvenTurboRoundEntity))).Find(x => x.Number == roundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var records = await GetBaseAggregateBuilder<OddEvenTurboBetEntity, OddEvenBetWithUnwindAccountsEntity>(roundId, nickname, excludeBot)
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

            return (new OddEvenTurboRoundEntity(), Enumerable.Empty<OddEvenBetReportEntity>());
        }
    }
}