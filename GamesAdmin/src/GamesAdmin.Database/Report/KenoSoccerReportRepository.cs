using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IKenoSoccerReportRepository
    {
        Task<(TRoundEntity, IEnumerable<KenoSoccerBetReportEntity>)> GetAsync<TRoundEntity, TOneEntity>(long roundId, string nickname, bool excludeUUS)
                where TRoundEntity : KenoSoccerRoundEntity, new()
                where TOneEntity : KenoSoccerOneEntity;
    }

    public class KenoSoccerReportRepository : BaseBetReportRepository, IKenoSoccerReportRepository

    {
        public KenoSoccerReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(TRoundEntity, IEnumerable<KenoSoccerBetReportEntity>)> GetAsync<TRoundEntity, TOneEntity>(long roundId, string nickname, bool excludeUUS)
            where TRoundEntity : KenoSoccerRoundEntity, new()
            where TOneEntity : KenoSoccerOneEntity
        {
            var roundResult = await database.GetCollection<TRoundEntity>(Document.GetCollectionName(typeof(TRoundEntity)))
                .Find(x => x.Number == roundId && x.FullTime != null).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<TOneEntity, KenoSoccerBetWithUnwindAccountsEntity>(roundId, nickname, true, excludeUUS)
                    .Project(x => new KenoSoccerBetReportEntity
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

            return (new TRoundEntity(), Enumerable.Empty<KenoSoccerBetReportEntity>());
        }
    }
}