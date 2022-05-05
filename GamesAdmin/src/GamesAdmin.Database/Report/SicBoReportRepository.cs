using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface ISicBoReportRepository
    {
        Task<(SicBoRoundEntity, IEnumerable<SicBoBetReportEntity>)> GetAsync(BetReportFilter filter);
    }

    public class SicBoReportRepository : BaseBetReportRepository, ISicBoReportRepository
    {
        public SicBoReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(SicBoRoundEntity, IEnumerable<SicBoBetReportEntity>)> GetAsync(BetReportFilter filter)
        {
            var roundResult = await database.GetCollection<SicBoRoundEntity>(Document.GetCollectionName(typeof(SicBoRoundEntity)))
                .Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<SicBoOnesEntity, SicBoBetWithUnwindAccountsEntity>(
                    filter.RoundId, filter.Nickname, filter.ExcludeBot, filter.ExcludeUUS, filter.PartnerId, filter.Username)
                    .Project(x => new SicBoBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        Nickname = x.AccountInfos.Name,
                        BetWin = x.Betwin,
                        TotalBet = x.TotalAmount,
                        UserId = x.Uid,
                        Currency = x.Currency,
                        Rate = x.Rate,
                        PartnerId = x.OperatorId
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new SicBoRoundEntity(), Enumerable.Empty<SicBoBetReportEntity>());
        }
    }
}
