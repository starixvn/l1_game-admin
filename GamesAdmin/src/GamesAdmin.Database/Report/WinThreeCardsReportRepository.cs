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

        Task<(WinThreeCardsRoundEntity, IEnumerable<WinThreeCardsBetLogEntity>, Dictionary<string, decimal>)> GetBetHistoryAsync(BetHistoryFilter filter);
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
                        UserId = x.Uid,
                        Nickname = x.AccountInfos.Name,
                        BetWin = x.Betwin,
                        TotalBet = x.Dragon + x.Phoenix + x.ThreeKind + x.StraightFlush + x.Flush + x.Pair8Plus + x.Straight,
                        Currency = x.Currency,
                        Rate = x.Rate,
                        OperatorId = x.OperatorId,
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new WinThreeCardsRoundEntity(), Enumerable.Empty<WinThreeCardsBetReportEntity>());
        }

        public async Task<(WinThreeCardsRoundEntity, IEnumerable<WinThreeCardsBetLogEntity>, Dictionary<string, decimal>)> GetBetHistoryAsync(BetHistoryFilter filter)
        {
            var round = await database.GetCollection<WinThreeCardsRoundEntity>(Document.GetCollectionName(typeof(WinThreeCardsRoundEntity)))
                .Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();
            var bet = await database.GetCollection<WinThreeCardsBetEntity>(Document.GetCollectionName(typeof(WinThreeCardsBetEntity)))
               .Find(x => x.Round == filter.RoundId && x.Uid == filter.UserId && x.OperatorId == filter.PartnerId).FirstOrDefaultAsync();
            var betLogs = await database.GetCollection<WinThreeCardsBetLogEntity>(Document.GetCollectionName(typeof(WinThreeCardsBetLogEntity)))
               .Find(x => x.Round == filter.RoundId && x.Uid == filter.UserId && x.OperatorId == filter.PartnerId).SortByDescending(x => x.Id).ToListAsync();

            return (round, betLogs, bet.Odds);
        }
    }
}