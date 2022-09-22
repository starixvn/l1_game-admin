using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database.Report
{
    public interface IStakeBalancedReportRepository
    {
        Task<(TRoundEntity, IEnumerable<StakeBalancedBetReportEntity>)> GetAsync<TRoundEntity, TBetEntity>(long roundId, string nickname, bool excludeBot, string partnerId, string username, string defaultChoice)
            where TRoundEntity : BaseRoundEntity, new()
            where TBetEntity : BasePoolBetEntity;
    }

    public class StakeBalancedReportRepository : BaseBetReportRepository, IStakeBalancedReportRepository
    {
        public StakeBalancedReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(TRoundEntity, IEnumerable<StakeBalancedBetReportEntity>)> GetAsync<TRoundEntity, TBetEntity>(
            long roundId, string nickname, bool excludeBot, string partnerId, string username, string defaultChoice)
            where TRoundEntity : BaseRoundEntity, new()
            where TBetEntity : BasePoolBetEntity
        {
            var roundResult = await database.GetCollection<TRoundEntity>(Document.GetCollectionName(typeof(TRoundEntity)))
                .Find(x => x.Number == roundId)
                .FirstOrDefaultAsync();

            if (roundResult == null)
            {
                return (new TRoundEntity(), Enumerable.Empty<StakeBalancedBetReportEntity>());
            }

            var project = GetBaseAggregateBuilder<TBetEntity, StakeBalancedBetWithUnwindAccountsEntity>(roundId, nickname, excludeBot, true, partnerId, username)
               .Project(x => new StakeBalancedBetReportEntity
               {
                   Time = x.Time,
                   Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                   Nickname = x.AccountInfos.Name,
                   Select = x.Select ?? x.Choice == defaultChoice,
                   Bet = x.BaseAmount,
                   BetWin = x.BaseBetWin,
                   Back = x.BaseRefund,
                   Choice = x.Choice,
                   UserId = x.Uid,
                   Win = x.Win,
                   Currency = x.Currency,
                   PartnerId = x.OperatorId,
                   Rate = x.Rate
               });

            var records = await project.ToListAsync();
            return (roundResult, records);
        }
    }
}
