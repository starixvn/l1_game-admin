using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface ISicBoReportRepository
    {
        Task<(SicBoRoundEntity, IEnumerable<SicBoBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeUUS);
    }

    public class SicBoReportRepository : BaseBetReportRepository, ISicBoReportRepository
    {
        public SicBoReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(SicBoRoundEntity, IEnumerable<SicBoBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeUUS)
        {
            var roundResult = await database.GetCollection<SicBoRoundEntity>(Document.GetCollectionName(typeof(SicBoRoundEntity)))
                .Find(x => x.Number == roundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<SicBoOnesEntity, SicBoBetWithUnwindAccountsEntity>(roundId, nickname, excludeUUS: excludeUUS)
                    .Project(x => new SicBoBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        Nickname = x.AccountInfos.Name,
                        BetWin = x.Betwin,
                        TotalBet = x.TotalAmount,
                        UserId = x.Uid,
                        Currency = x.Currency,
                        Rate = x.Rate,
                        PartnerId = x.PartnerId
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new SicBoRoundEntity(), Enumerable.Empty<SicBoBetReportEntity>());
        }
    }
}
