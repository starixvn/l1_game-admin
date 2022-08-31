using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database.Report
{
    public interface IFruitSlotReportRepository
    {
        Task<(FruitSlotRoundEntity, IEnumerable<FruitSlotBetReportEntity>)> GetRecordAsync(BetReportFilter filter);

        Task<(FruitSlotRoundEntity, IEnumerable<FruitSlotBetLogEntity>, Dictionary<string, decimal> Odds)> GetBetHistoryAsync(BetHistoryFilter filter);
    }

    public class FruitSlotReportRepository : BaseBetReportRepository, IFruitSlotReportRepository
    {
        public FruitSlotReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(FruitSlotRoundEntity, IEnumerable<FruitSlotBetReportEntity>)> GetRecordAsync(BetReportFilter filter)
        {
            var roundResult = await database.GetCollection<FruitSlotRoundEntity>(Document.GetCollectionName(typeof(FruitSlotRoundEntity)))
                .Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<FruitSlotBetEntity, FruitSlotBetWithUnWindAccountsEntity>(
                    filter.RoundId, filter.Nickname, filter.ExcludeBot, filter.ExcludeUUS, filter.PartnerId, filter.Username)
                    .Project(x => new FruitSlotBetReportEntity
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

            return (new FruitSlotRoundEntity(), Enumerable.Empty<FruitSlotBetReportEntity>());
        }

        public async Task<(FruitSlotRoundEntity, IEnumerable<FruitSlotBetLogEntity>, Dictionary<string, decimal> Odds)> GetBetHistoryAsync(BetHistoryFilter filter)
        {
            var roundResult = await database.GetCollection<FruitSlotRoundEntity>(Document.GetCollectionName(typeof(FruitSlotRoundEntity)))
            .Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();
            var bet = await database.GetCollection<FruitSlotBetEntity>(Document.GetCollectionName(typeof(FruitSlotBetEntity)))
                .Find(x => x.Round == filter.RoundId && x.Uid == filter.UserId && x.OperatorId == filter.PartnerId).FirstOrDefaultAsync();

            var betLogs = await database.GetCollection<FruitSlotBetLogEntity>(Document.GetCollectionName(typeof(FruitSlotBetLogEntity)))
                .Find(x => x.Round == filter.RoundId && x.Uid == filter.UserId && x.OperatorId == filter.PartnerId).SortByDescending(x => x.Id).ToListAsync();

            return (roundResult, betLogs, bet.Odds);
        }
    }
}