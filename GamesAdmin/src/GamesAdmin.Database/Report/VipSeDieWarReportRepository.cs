using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IVipSeDieWarReportRepository
    {
        Task<(VipSeDieWarRoundEntity, IEnumerable<VipSeDieWarBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeUUS);
    }

    public class VipSeDieWarReportRepository : BaseBetReportRepository, IVipSeDieWarReportRepository
    {
        public VipSeDieWarReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(VipSeDieWarRoundEntity, IEnumerable<VipSeDieWarBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeUUS)
        {
            var roundResult = await database.GetCollection<VipSeDieWarRoundEntity>(Document.GetCollectionName(typeof(VipSeDieWarRoundEntity))).Find(x => x.Number == roundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<VipSeDieWarOnesEntity, VipSeDieWarBetWithUnWindAccountsEntity>(roundId, nickname, excludeUUS: excludeUUS)
                    .Project(x => new VipSeDieWarBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        Nickname = x.AccountInfos.Name,
                        UserId = x.Uid,
                        BetWin = x.Betwin,
                        TotalBet = x.TotalAmount
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new VipSeDieWarRoundEntity(), Enumerable.Empty<VipSeDieWarBetReportEntity>());
        }
    }
}