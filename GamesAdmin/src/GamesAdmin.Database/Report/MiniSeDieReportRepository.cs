using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IMiniSeDieReportRepository
    {
        Task<(MiniSeDieRoundEntity, IEnumerable<SeDieBetReportEntity>)> GetAsync(BetReportFilter filter);
    }

    public class MiniSeDieReportRepository : BaseBetReportRepository, IMiniSeDieReportRepository
    {
        public MiniSeDieReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(MiniSeDieRoundEntity, IEnumerable<SeDieBetReportEntity>)> GetAsync(BetReportFilter filter)
        {
            var roundResult = await database.GetCollection<MiniSeDieRoundEntity>(Document.GetCollectionName(typeof(MiniSeDieRoundEntity))).Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<MiniSeDieBetEntity, MiniSeDieBetWithUnwindAccountsEntity>(
                    filter.RoundId, filter.Nickname, filter.ExcludeBot, filter.ExcludeUUS, filter.PartnerId, filter.Username)
                    .Project(x => new SeDieBetReportEntity
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
                        Currency = x.Currency,
                        Rate = x.Rate,
                        PartnerId = x.OperatorId
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new MiniSeDieRoundEntity(), Enumerable.Empty<SeDieBetReportEntity>());
        }
    }
}