using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface ILotteryReportRepository
    {
        Task<(LotteryRoundEntity, IEnumerable<LotteryBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeUUS);
    }

    public class LotteryReportRepository : BaseBetReportRepository, ILotteryReportRepository
    {
        public LotteryReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(LotteryRoundEntity, IEnumerable<LotteryBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeUUS)
        {
            var roundResult = await database.GetCollection<LotteryRoundEntity>(Document.GetCollectionName(typeof(LotteryRoundEntity)))
                .Find(x => x.Number == roundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<LotteryOnesEntity, LotteryBetWithUnwindAccountsEntity>(roundId, nickname, true,  excludeUUS)
                    .Project(x => new LotteryBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        Nickname = x.AccountInfos.Name,
                        BetWin = x.Betwin,
                        TotalBet = x.TotalAmount,
                        UserId = x.Uid
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new LotteryRoundEntity(), Enumerable.Empty<LotteryBetReportEntity>());
        }
    }
}
