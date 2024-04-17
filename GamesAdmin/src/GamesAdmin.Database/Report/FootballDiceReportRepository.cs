using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IFootballDiceReportRepository
    {
        Task<(FootballDiceRoundEntity, IEnumerable<FootballDiceBetReportEntity>)> GetRecordAsync(BetReportFilter filter);

        Task<(FootballDiceRoundEntity, IEnumerable<FootballDiceBetLogEntity>, Dictionary<string, decimal> Odds)> GetBetHistoryAsync(BetHistoryFilter filter);
    }

    public class FootballDiceReportRepository : BaseBetReportRepository, IFootballDiceReportRepository
    {
        public FootballDiceReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(FootballDiceRoundEntity, IEnumerable<FootballDiceBetReportEntity>)> GetRecordAsync(BetReportFilter filter)
        {
            var roundResult = await database.GetCollection<FootballDiceRoundEntity>(Document.GetCollectionName(typeof(FootballDiceRoundEntity)))
                .Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<FootballDiceBetEntity, FootballDiceBetWithUnWindAccountsEntity>(
                        filter.RoundId, filter.Nickname, filter.ExcludeBot, filter.ExcludeUUS, filter.PartnerId, filter.Username)
                    .Project(x => new FootballDiceBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        UserId = x.Uid,
                        Nickname = x.AccountInfos.Name,
                        Betwin = x.Betwin,
                        Home = x.Home,
                        Away = x.Away,
                        Draw = x.Draw,
                        TotalAmount = x.TotalAmount,
                        Currency = x.Currency,
                        Rate = x.Rate,
                        OperatorId = x.OperatorId
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new FootballDiceRoundEntity(), Enumerable.Empty<FootballDiceBetReportEntity>());
        }

        public async Task<(FootballDiceRoundEntity, IEnumerable<FootballDiceBetLogEntity>, Dictionary<string, decimal> Odds)> GetBetHistoryAsync(BetHistoryFilter filter)
        {
            var roundResult = await database.GetCollection<FootballDiceRoundEntity>(Document.GetCollectionName(typeof(FootballDiceRoundEntity)))
                .Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();
            var bet = await database.GetCollection<FootballDiceBetEntity>(Document.GetCollectionName(typeof(FootballDiceBetEntity)))
                .Find(x => x.Round == filter.RoundId && x.Uid == filter.UserId && x.OperatorId == filter.PartnerId).FirstOrDefaultAsync();

            var betLogs = await database.GetCollection<FootballDiceBetLogEntity>(Document.GetCollectionName(typeof(FootballDiceBetLogEntity)))
                .Find(x => x.Round == filter.RoundId && x.Uid == filter.UserId && x.OperatorId == filter.PartnerId).SortByDescending(x => x.Id).ToListAsync();

            return (roundResult, betLogs, bet.Odds);
        }
    }
}