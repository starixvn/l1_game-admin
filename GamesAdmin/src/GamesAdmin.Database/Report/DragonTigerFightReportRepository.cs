using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Entities.DragonTigerFight;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database.Report
{
    public interface IDragonTigerFightReportRepository
    {
        Task<(DragonTigerFightRoundEntity, IEnumerable<DragonTigerFightBetReportEntity>)> GetRecordAsync(BetReportFilter filter);

        Task<(DragonTigerFightRoundEntity, IEnumerable<DragonTigerFightBetLogEntity>, Dictionary<string, decimal> Odds)> GetBetHistoryAsync(BetHistoryFilter filter);
    }

    public class DragonTigerFightReportRepository : BaseBetReportRepository, IDragonTigerFightReportRepository
    {
        public DragonTigerFightReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(DragonTigerFightRoundEntity, IEnumerable<DragonTigerFightBetReportEntity>)> GetRecordAsync(BetReportFilter filter)
        {
            var roundResult = await database.GetCollection<DragonTigerFightRoundEntity>(Document.GetCollectionName(typeof(DragonTigerFightRoundEntity)))
                .Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<DragonTigerFightBetEntity, DragonTigerFightBetWithUnWindAccountsEntity>(
                        filter.RoundId, filter.Nickname, filter.ExcludeBot, filter.ExcludeUUS, filter.PartnerId, filter.Username)
                    .Project(x => new DragonTigerFightBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        UserId = x.Uid,
                        Nickname = x.AccountInfos.Name,
                        Betwin = x.Betwin,
                        Dragon = x.Dragon,
                        BlackDragon = x.BlackDragon,
                        RedDragon = x.RedDragon,
                        Tiger = x.Tiger,
                        BlackTiger = x.BlackTiger,
                        RedTiger = x.RedTiger,
                        Tie = x.Tie,
                        TotalAmount = x.TotalAmount,
                        Currency = x.Currency,
                        Rate = x.Rate,
                        OperatorId = x.OperatorId
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new DragonTigerFightRoundEntity(), Enumerable.Empty<DragonTigerFightBetReportEntity>());
        }

        public async Task<(DragonTigerFightRoundEntity, IEnumerable<DragonTigerFightBetLogEntity>, Dictionary<string, decimal> Odds)> GetBetHistoryAsync(BetHistoryFilter filter)
        {
            var roundResult = await database.GetCollection<DragonTigerFightRoundEntity>(Document.GetCollectionName(typeof(DragonTigerFightRoundEntity)))
                .Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();
            var bet = await database.GetCollection<DragonTigerFightBetEntity>(Document.GetCollectionName(typeof(DragonTigerFightBetEntity)))
                .Find(x => x.Round == filter.RoundId && x.Uid == filter.UserId && x.OperatorId == filter.PartnerId).FirstOrDefaultAsync();

            var betLogs = await database.GetCollection<DragonTigerFightBetLogEntity>(Document.GetCollectionName(typeof(DragonTigerFightBetLogEntity)))
                .Find(x => x.Round == filter.RoundId && x.Uid == filter.UserId && x.OperatorId == filter.PartnerId).SortByDescending(x => x.Id).ToListAsync();

            return (roundResult, betLogs, bet.Odds);
        }
    }
}