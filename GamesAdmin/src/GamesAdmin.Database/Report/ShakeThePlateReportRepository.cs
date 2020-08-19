using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IShakeThePlateReportRepository
    {
        Task<(SeDieRoundEntity, IEnumerable<ShakeThePlateBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeUUS);
    }

    public class ShakeThePlateReportRepository : BaseBetReportRepository, IShakeThePlateReportRepository
    {
        public ShakeThePlateReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(SeDieRoundEntity, IEnumerable<ShakeThePlateBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeUUS)
        {
            var roundResult = await database.GetCollection<SeDieRoundEntity>(Document.GetCollectionName(typeof(SeDieRoundEntity))).Find(x => x.Number == roundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<SeDieBetEntity, ShakeThePlateBetWithUnwindAccountsEntity>(roundId, nickname, excludeUUS: excludeUUS)
                    .Project(x => new ShakeThePlateBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        Nickname = x.AccountInfos.Name,
                        BetWin = x.Betwin,
                        Even = x.Even + x.OldEven,
                        Odd = x.Odd + x.OldOdd,
                        ThreeRed = x.ThreeRed,
                        FourRed = x.FourRed,
                        ThreeWhite = x.ThreeWhite,
                        FourWhite = x.FourWhite
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new SeDieRoundEntity(), Enumerable.Empty<ShakeThePlateBetReportEntity>());
        }
    }
}