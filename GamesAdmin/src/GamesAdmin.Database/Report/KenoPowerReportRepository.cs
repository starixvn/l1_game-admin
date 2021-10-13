using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IKenoPowerReportRepository
    {
        Task<(TRoundEntity, IEnumerable<KenoPowerBetReportEntity>)> GetAsync<TRoundEntity, TOneEntity>(long roundId, string nickname, bool excludeUUS)
                where TRoundEntity : KenoPowerRoundEntity, new()
                where TOneEntity : KenoPowerOneEntity;
    }

    public class KenoPowerReportRepository : BaseBetReportRepository, IKenoPowerReportRepository

    {
        public KenoPowerReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(TRoundEntity, IEnumerable<KenoPowerBetReportEntity>)> GetAsync<TRoundEntity, TOneEntity>(long roundId, string nickname, bool excludeUUS)
            where TRoundEntity : KenoPowerRoundEntity, new()
            where TOneEntity : KenoPowerOneEntity
        {
            var roundResult = await database.GetCollection<TRoundEntity>(Document.GetCollectionName(typeof(TRoundEntity)))
                .Find(x => x.Number == roundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<TOneEntity, KenoPowerBetWithUnwindAccountsEntity>(roundId, nickname, true, excludeUUS)
                    .Project(x => new KenoPowerBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        Nickname = x.AccountInfos.Name,
                        BetWin = x.Betwin,
                        TotalBet = x.TotalAmount,
                        Refund = x.Refund,
                        UserId = x.Uid
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new TRoundEntity(), Enumerable.Empty<KenoPowerBetReportEntity>());
        }
    }
}