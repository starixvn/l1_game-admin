using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IVipSeDieWarReportRepository
    {
        Task<(VipSeDieWarRoundEntity, IEnumerable<VipSeDieWarBetReportEntity>)> GetAsync(BetReportFilter filter);
    }

    public class VipSeDieWarReportRepository : BaseBetReportRepository, IVipSeDieWarReportRepository
    {
        public VipSeDieWarReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(VipSeDieWarRoundEntity, IEnumerable<VipSeDieWarBetReportEntity>)> GetAsync(BetReportFilter filter)
        {
            var roundResult = await database.GetCollection<VipSeDieWarRoundEntity>(
                Document.GetCollectionName(typeof(VipSeDieWarRoundEntity))).Find(x => x.Number == filter.RoundId)
                .FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<VipSeDieWarOnesEntity, VipSeDieWarBetWithUnWindAccountsEntity>(
                    filter.RoundId, filter.Nickname, filter.ExcludeBot, filter.ExcludeUUS, filter.PartnerId, filter.Username)
                    .Project(x => new VipSeDieWarBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        Nickname = x.AccountInfos.Name,
                        UserId = x.Uid,
                        BetWin = x.Betwin,
                        TotalBet = x.TotalAmount,
                        Currency = x.Currency,
                        Rate = x.Rate,
                        PartnerId = x.PartnerId
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new VipSeDieWarRoundEntity(), Enumerable.Empty<VipSeDieWarBetReportEntity>());
        }
    }
}