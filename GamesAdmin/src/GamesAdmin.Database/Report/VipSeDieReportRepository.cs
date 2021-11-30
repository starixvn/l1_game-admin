using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IVipSeDieReportRepository
    {
        Task<(VipSeDieRoundEntity, IEnumerable<VipSeDieBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeUUS);
    }

    public class VipSeDieReportRepository : BaseBetReportRepository, IVipSeDieReportRepository
    {
        public VipSeDieReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(VipSeDieRoundEntity, IEnumerable<VipSeDieBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeUUS)
        {
            var roundResult = await database.GetCollection<VipSeDieRoundEntity>(Document.GetCollectionName(typeof(VipSeDieRoundEntity))).Find(x => x.Number == roundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<VipSeDieBetEntity, VipSeDieBetWithUnWindAccountsEntity>(roundId, nickname, excludeUUS: excludeUUS)
                    .Project(x => new VipSeDieBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        Nickname = x.AccountInfos.Name,
                        BetWin = x.Betwin,
                        Even = x.Even,
                        Odd = x.Odd,
                        TwoRed = x.TwoRed,
                        ThreeRed = x.ThreeRed,
                        FourRed = x.FourRed,
                        ThreeWhite = x.ThreeWhite,
                        FourWhite = x.FourWhite,
                        UserId = x.Uid,
                        Currency = x.Currency,
                        Rate = x.Rate,
                        PartnerId = x.PartnerId
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new VipSeDieRoundEntity(), Enumerable.Empty<VipSeDieBetReportEntity>());
        }
    }
}