using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IWin3CardsReportRepository
    {
        Task<(Win3CardsRoundEntity, IEnumerable<Win3CardsBetReportEntity>)> GetRecordAsync(BetReportFilter filter);

        Task<(Win3CardsRoundEntity, IEnumerable<Win3CardsBetEntity>, Dictionary<string, decimal> Odds)> GetBetHistoryAsync(BetHistoryFilter filter);
    }
    public class Win3CardsReportRepository : BaseBetReportRepository, IWin3CardsReportRepository
    {
        public Win3CardsReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(Win3CardsRoundEntity, IEnumerable<Win3CardsBetReportEntity>)> GetRecordAsync(BetReportFilter filter)
        {
            var roundResult = await database.GetCollection<Win3CardsRoundEntity>(Document.GetCollectionName(typeof(Win3CardsRoundEntity)))
                .Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<Win3CardsBetEntity, Win3CardsBetWithUnWindAccountsEntity>(
                    filter.RoundId, filter.Nickname, filter.ExcludeBot, filter.ExcludeUUS, filter.PartnerId, filter.Username)
                    .Project(x => new Win3CardsBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        Nickname = x.AccountInfos.Name,
                        BetWin = x.Betwin,
                        TotalBet = x.Dragon + x.Phoenix + x.ThreeKind + x.StraightFlush + x.FLush + x.Pair8Plus + x.Straight,
                        Currency = x.Currency,
                        Rate = x.Rate,
                        PartnerId = x.OperatorId
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new Win3CardsRoundEntity(), Enumerable.Empty<Win3CardsBetReportEntity>());
        }

        public async Task<(Win3CardsRoundEntity, IEnumerable<Win3CardsBetEntity>, Dictionary<string, decimal> Odds)> GetBetHistoryAsync(BetHistoryFilter filter)
        {
            var roundCollection = database.GetCollection<Win3CardsRoundEntity>(Document.GetCollectionName(typeof(Win3CardsRoundEntity)));
            var roundResult = await roundCollection.Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();
          
            var betCollection = database.GetCollection<Win3CardsBetEntity>(Document.GetCollectionName(typeof(Win3CardsBetEntity)));
            var bets = await betCollection.Find(x => x.Round == filter.RoundId && x.Uid == filter.UserId && x.OperatorId == filter.PartnerId).SortByDescending(x => x.Id).ToListAsync();
            var bet = await betCollection.Find(x => x.Round == filter.RoundId && x.Uid == filter.UserId && x.OperatorId == filter.PartnerId).FirstOrDefaultAsync();

            return (roundResult, bets, bet.Odds);
        }
    }
}