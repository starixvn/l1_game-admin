using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IMiniSeDieMultiplayReportRepository
    {
        Task<(TRoundEntity, IEnumerable<SeDieBetReportEntity>)> GetAsync<TRoundEntity>(BetReportFilter filter)
            where TRoundEntity : MiniSeDieMultiplayRoundEntity, new();
    }

    public class MiniSeDieMultiplayReportRepository : BaseBetReportRepository, IMiniSeDieMultiplayReportRepository
    {
        public MiniSeDieMultiplayReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(TRoundEntity, IEnumerable<SeDieBetReportEntity>)> GetAsync<TRoundEntity>(BetReportFilter filter)
            where TRoundEntity : MiniSeDieMultiplayRoundEntity, new()
        {
            var roundResult = await database.GetCollection<TRoundEntity>(Document.GetCollectionName(typeof(TRoundEntity))).Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<MiniSeDieMultiplayBetEntity, MiniSeDieMultiplayBetWithUnwindAccountsEntity>(
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

            return (new TRoundEntity(), Enumerable.Empty<SeDieBetReportEntity>());
        }
    }
}