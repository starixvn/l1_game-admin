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
        Task<(OddEvenTurboRoundEntity, IEnumerable<OddEvenBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeBot, string partnerId, string username);
    }

    public class OddEvenTurboReportRepository : BaseBetReportRepository, IOddEvenTurboReportRepository
    {
        public OddEvenTurboReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(OddEvenTurboRoundEntity, IEnumerable<OddEvenBetReportEntity>)> GetAsync(
            long roundId,
            string nickname,
            bool excludeBot,
            string partnerId,
            string username)
        {
            var roundResult = await database.GetCollection<OddEvenTurboRoundEntity>(Document.GetCollectionName(typeof(OddEvenTurboRoundEntity))).Find(x => x.Number == roundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var records = await GetBaseAggregateBuilder<OddEvenTurboBetEntity, OddEvenBetWithUnwindAccountsEntity>(roundId, nickname, excludeBot, true, partnerId, username)
                    .Project(x => new OddEvenBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        Nickname = x.AccountInfos.Name,
                        Select = x.Select ?? x.Choice == "even",
                        Bet = x.Bet ?? x.Amount,
                        BetWin = x.Betwin,
                        Back = x.Back ?? x.Refund,
                        Currency = x.Currency,
                        PartnerId = x.PartnerId,
                        Rate = x.Rate
                    }).ToListAsync();

                return (roundResult, records);
            }

            return (new OddEvenTurboRoundEntity(), Enumerable.Empty<OddEvenBetReportEntity>());
        }
    }
}