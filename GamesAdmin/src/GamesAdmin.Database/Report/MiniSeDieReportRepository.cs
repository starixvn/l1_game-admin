using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IMiniSeDieReportRepository
    {
        Task<(MiniSeDieRoundEntity, IEnumerable<ShakeThePlateBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeBot);
    }

    public class MiniSeDieReportRepository : BaseBetReportRepository, IMiniSeDieReportRepository
    {
        public MiniSeDieReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(MiniSeDieRoundEntity, IEnumerable<ShakeThePlateBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeBot)
        {
            var roundResult = await database.GetCollection<MiniSeDieRoundEntity>(Document.GetCollectionName(typeof(MiniSeDieRoundEntity))).Find(x => x.Number == roundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<MiniSeDieBetEntity, MiniSeDieBetWithUnwindAccountsEntity>(roundId, nickname, excludeBot)
                    .Project(x => new ShakeThePlateBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        Nickname = x.AccountInfos.Name,
                        BetWin = x.Betwin,
                        Even = x.Even,
                        Odd = x.Odd,
                        ThreeRed = x.ThreeRed,
                        FourRed = x.FourRed,
                        ThreeWhite = x.ThreeWhite,
                        FourWhite = x.FourWhite
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new MiniSeDieRoundEntity(), Enumerable.Empty<ShakeThePlateBetReportEntity>());
        }
    }
}