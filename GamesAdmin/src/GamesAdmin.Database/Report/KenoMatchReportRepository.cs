using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IKenoMatchReportRepository
    {
        Task<(TRoundEntity, IEnumerable<KenoMatchBetReportEntity>)> GetAsync<TRoundEntity, TOneEntity>(long roundId, string nickname, bool excludeUUS)
                where TRoundEntity : KenoMatchRoundEntity, new()
                where TOneEntity : KenoMatchOneEntity;
    }

    public class KenoMatchReportRepository : BaseBetReportRepository, IKenoMatchReportRepository

    {
        public KenoMatchReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(TRoundEntity, IEnumerable<KenoMatchBetReportEntity>)> GetAsync<TRoundEntity, TOneEntity>(long roundId, string nickname, bool excludeUUS)
            where TRoundEntity : KenoMatchRoundEntity, new()
            where TOneEntity : KenoMatchOneEntity
        {
            var roundResult = await database.GetCollection<TRoundEntity>(Document.GetCollectionName(typeof(TRoundEntity)))
                .Find(x => x.Number == roundId && x.FullTime != null).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<TOneEntity, KenoMatchBetWithUnwindAccountsEntity>(roundId, nickname, true, excludeUUS)
                    .Project(x => new KenoMatchBetReportEntity
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

            return (new TRoundEntity(), Enumerable.Empty<KenoMatchBetReportEntity>());
        }
    }
}