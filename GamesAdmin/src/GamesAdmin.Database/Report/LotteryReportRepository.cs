using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface ILotteryReportRepository
    {
        Task<(LotteryRoundEntity, IEnumerable<LotteryBetReportEntity>)> GetAsync(BetReportFilter filter);
    }

    public class LotteryReportRepository : BaseBetReportRepository, ILotteryReportRepository
    {
        public LotteryReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(LotteryRoundEntity, IEnumerable<LotteryBetReportEntity>)> GetAsync(BetReportFilter filter)
        {
            var roundResult = await database.GetCollection<LotteryRoundEntity>(Document.GetCollectionName(typeof(LotteryRoundEntity)))
                .Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<LotteryOnesEntity, LotteryBetWithUnwindAccountsEntity>(
                    filter.RoundId, filter.Nickname, filter.ExcludeBot, filter.ExcludeUUS, filter.PartnerId, filter.Username)
                    .Project(x => new LotteryBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        Nickname = x.AccountInfos.Name,
                        BetWin = x.Betwin,
                        TotalBet = x.TotalAmount,
                        UserId = x.Uid,
                        Currency = x.Currency,
                        Rate = x.Rate,
                        PartnerId = x.OperatorId
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new LotteryRoundEntity(), Enumerable.Empty<LotteryBetReportEntity>());
        }
    }
}
