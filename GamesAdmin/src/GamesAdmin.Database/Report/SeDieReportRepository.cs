using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface ISeDieReportRepository
    {
        Task<(SeDieRoundEntity, IEnumerable<SeDieBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeUUS);
    }

    public class SeDieReportRepository : BaseBetReportRepository, ISeDieReportRepository
    {
        public SeDieReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(SeDieRoundEntity, IEnumerable<SeDieBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeUUS)
        {
            var roundResult = await database.GetCollection<SeDieRoundEntity>(Document.GetCollectionName(typeof(SeDieRoundEntity))).Find(x => x.Number == roundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<SeDieBetEntity, SeDieBetWithUnwindAccountsEntity>(roundId, nickname, excludeUUS: excludeUUS);

                var records = (await project.ToListAsync())
                    .Select(x => new SeDieBetReportEntity
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
                        FourWhite = x.FourWhite,
                        TwoRed = x.TwoRed,
                        FreeBet = x.FreeBet ?? false
                    });

                return (roundResult, records);
            }

            return (new SeDieRoundEntity(), Enumerable.Empty<SeDieBetReportEntity>());
        }
    }
}