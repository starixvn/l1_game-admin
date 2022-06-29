using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IVipSeDieReportRepository
    {
        Task<(VipSeDieRoundEntity, IEnumerable<VipSeDieBetReportEntity>)> GetAsync(BetReportFilter filter);
    }

    public class VipSeDieReportRepository : BaseBetReportRepository, IVipSeDieReportRepository
    {
        public VipSeDieReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(VipSeDieRoundEntity, IEnumerable<VipSeDieBetReportEntity>)> GetAsync(BetReportFilter filter)
        {
            var roundResult = await database.GetCollection<VipSeDieRoundEntity>(Document.GetCollectionName(typeof(VipSeDieRoundEntity))).Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<VipSeDieBetEntity, VipSeDieBetWithUnWindAccountsEntity>(
                    filter.RoundId, filter.Nickname, filter.ExcludeBot, filter.ExcludeUUS, filter.PartnerId, filter.Username)
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
                        PartnerId = x.OperatorId
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new VipSeDieRoundEntity(), Enumerable.Empty<VipSeDieBetReportEntity>());
        }
    }
}