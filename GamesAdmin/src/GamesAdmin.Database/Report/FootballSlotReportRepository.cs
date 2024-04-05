using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database.Report
{
    public interface IFootballSlotReportRepository
    {
        Task<(FootballSlotRoundEntity, IEnumerable<FootballSlotBetReportEntity>)> GetRecordAsync(BetReportFilter filter);

        Task<(FootballSlotRoundEntity, IEnumerable<FootballSlotBetLogEntity>, Dictionary<string, decimal> Odds)> GetBetHistoryAsync(BetHistoryFilter filter);
    }

    public class FootballSlotReportRepository : BaseBetReportRepository, IFootballSlotReportRepository
    {
        public FootballSlotReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(FootballSlotRoundEntity, IEnumerable<FootballSlotBetReportEntity>)> GetRecordAsync(BetReportFilter filter)
        {
            var roundResult = await database.GetCollection<FootballSlotRoundEntity>(Document.GetCollectionName(typeof(FootballSlotRoundEntity)))
                .Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<FootballSlotBetEntity, FootballSlotBetWithUnWindAccountsEntity>(
                    filter.RoundId, filter.Nickname, filter.ExcludeBot, filter.ExcludeUUS, filter.PartnerId, filter.Username)
                    .Project(x => new FootballSlotBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        UserId = x.Uid,
                        Nickname = x.AccountInfos.Name,
                        Betwin = x.Betwin,
                        TotalAmount = x.TotalAmount,
                        Currency = x.Currency,
                        Rate = x.Rate,
                        OperatorId = x.OperatorId
                    });

                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new FootballSlotRoundEntity(), Enumerable.Empty<FootballSlotBetReportEntity>());
        }

        public async Task<(FootballSlotRoundEntity, IEnumerable<FootballSlotBetLogEntity>, Dictionary<string, decimal> Odds)> GetBetHistoryAsync(BetHistoryFilter filter)
        {
            var roundResult = await database.GetCollection<FootballSlotRoundEntity>(Document.GetCollectionName(typeof(FootballSlotRoundEntity)))
            .Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();
            var bet = await database.GetCollection<FootballSlotBetEntity>(Document.GetCollectionName(typeof(FootballSlotBetEntity)))
                .Find(x => x.Round == filter.RoundId && x.Uid == filter.UserId && x.OperatorId == filter.PartnerId).FirstOrDefaultAsync();

            var betLogs = await database.GetCollection<FootballSlotBetLogEntity>(Document.GetCollectionName(typeof(FootballSlotBetLogEntity)))
                .Find(x => x.Round == filter.RoundId && x.Uid == filter.UserId && x.OperatorId == filter.PartnerId).SortByDescending(x => x.Id).ToListAsync();

            return (roundResult, betLogs, bet.Odds);
        }
    }
}