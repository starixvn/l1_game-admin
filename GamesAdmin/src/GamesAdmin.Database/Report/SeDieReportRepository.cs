using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface ISeDieReportRepository
    {
        Task<(BaseRoundEntity, IEnumerable<BetReportEntity>)> GetAsync(BetReportFilter filter);
    }

    public class SeDieReportRepository : BaseBetReportRepository, ISeDieReportRepository
    {
        public SeDieReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(BaseRoundEntity, IEnumerable<BetReportEntity>)> GetAsync(BetReportFilter filter)
        {
            var roundResult = await database.GetCollection<SeDieRoundEntity>(Document.GetCollectionName(typeof(SeDieRoundEntity))).Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<SeDieBetEntity, SeDieBetWithUnwindAccountsEntity>(
                    filter.RoundId, filter.Nickname, filter.ExcludeBot, filter.ExcludeUUS, filter.PartnerId, filter.Username);

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
                        FreeBet = x.FreeBet ?? false,
                        Currency = x.Currency,
                        PartnerId = x.PartnerId,
                        Rate = x.Rate
                    });

                return (roundResult, records);
            }

            return (new SeDieRoundEntity(), Enumerable.Empty<SeDieBetReportEntity>());
        }
    }
}