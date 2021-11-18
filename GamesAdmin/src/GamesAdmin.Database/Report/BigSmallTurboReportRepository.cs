using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IBigSmallTurboReportRepository
    {
        Task<(BigSmallTurboRoundEntity, IEnumerable<BigSmallBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeBot, string partnerId, string username);
    }

    public class BigSmallTurboReportRepository : BaseBetReportRepository, IBigSmallTurboReportRepository
    {
        public BigSmallTurboReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(BigSmallTurboRoundEntity, IEnumerable<BigSmallBetReportEntity>)> GetAsync(
            long roundId,
            string nickname,
            bool excludeBot,
            string partnerId,
            string username)
        {
            var roundResult = await database.GetCollection<BigSmallTurboRoundEntity>(Document.GetCollectionName(typeof(BigSmallTurboRoundEntity))).Find(x => x.Number == roundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var records = await GetBaseAggregateBuilder<BigSmallTurboBetEntity, BigSmallBetWithUnwindAccountsEntity>(roundId, nickname, excludeBot, true, partnerId, username)
                    .Project(x => new BigSmallBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        Nickname = x.AccountInfos.Name,
                        Select = x.Select ?? x.Choice == "big",
                        Bet = x.Bet ?? x.Amount,
                        BetWin = x.Betwin,
                        Back = x.Back ?? x.Refund,
                        Currency = x.Currency,
                        Partner = x.PartnerId,
                        Rate = x.Rate
                    }).ToListAsync();

                return (roundResult, records);
            }

            return (new BigSmallTurboRoundEntity(), Enumerable.Empty<BigSmallBetReportEntity>());
        }
    }
}