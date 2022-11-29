using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IWinThreeCardsReportRepository
    {
        Task<(WinThreeCardsRoundEntity, IEnumerable<WinThreeCardsBetReportEntity>)> GetRecordAsync(BetReportFilter filter);

        Task<(WinThreeCardsRoundEntity, IEnumerable<WinThreeCardsBetEntity>, Dictionary<string, decimal> Odds)> GetBetHistoryAsync(BetHistoryFilter filter);
    }
    public class WinThreeCardsReportRepository : BaseBetReportRepository, IWinThreeCardsReportRepository
    {
        public WinThreeCardsReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(WinThreeCardsRoundEntity, IEnumerable<WinThreeCardsBetReportEntity>)> GetRecordAsync(BetReportFilter filter)
        {
            var roundResult = await database.GetCollection<WinThreeCardsRoundEntity>(Document.GetCollectionName(typeof(WinThreeCardsRoundEntity)))
                .Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<WinThreeCardsBetEntity, WinThreeCardsBetWithUnWindAccountsEntity>(
                    filter.RoundId, filter.Nickname, filter.ExcludeBot, filter.ExcludeUUS, filter.PartnerId, filter.Username)
                    .Project(x => new WinThreeCardsBetReportEntity
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

            return (new WinThreeCardsRoundEntity(), Enumerable.Empty<WinThreeCardsBetReportEntity>());
        }

        public async Task<(WinThreeCardsRoundEntity, IEnumerable<WinThreeCardsBetEntity>, Dictionary<string, decimal> Odds)> GetBetHistoryAsync(BetHistoryFilter filter)
        {
            var roundCollection = database.GetCollection<WinThreeCardsRoundEntity>(Document.GetCollectionName(typeof(WinThreeCardsRoundEntity)));
            var roundResult = await roundCollection.Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();
          
            var betCollection = database.GetCollection<WinThreeCardsBetEntity>(Document.GetCollectionName(typeof(WinThreeCardsBetEntity)));
            var bets = await betCollection.Find(x => x.Round == filter.RoundId && x.Uid == filter.UserId && x.OperatorId == filter.PartnerId).SortByDescending(x => x.Id).ToListAsync();
            var bet = await betCollection.Find(x => x.Round == filter.RoundId && x.Uid == filter.UserId && x.OperatorId == filter.PartnerId).FirstOrDefaultAsync();

            return (roundResult, bets, bet.Odds);
        }
    }
}