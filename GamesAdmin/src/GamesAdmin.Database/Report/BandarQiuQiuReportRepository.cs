using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Database.Report
{
    public interface IBandarQiuQiuReportRepository
    {
        Task<(BandarQiuQiuRoundEntity, IEnumerable<BandarQiuQiuBetEntity>)> GetRecordAsync(BetReportFilter filter);

        Task<(BandarQiuQiuRoundEntity, IEnumerable<BandarQiuQiuBetLogEntity>, Dictionary<string, decimal> Odds)> GetBetHistoryAsync(BetHistoryFilter filter);
    }

    public class BandarQiuQiuReportRepository : BaseBetReportRepository, IBandarQiuQiuReportRepository
    {
        public BandarQiuQiuReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(BandarQiuQiuRoundEntity, IEnumerable<BandarQiuQiuBetEntity>)> GetRecordAsync(BetReportFilter filter)
        {
            var roundResult = await database.GetCollection<BandarQiuQiuRoundEntity>(Document.GetCollectionName(typeof(BandarQiuQiuRoundEntity)))
                .Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var bets = await GetBets<BandarQiuQiuBetEntity>(filter.RoundId, filter.Nickname, filter.ExcludeUUS, filter.PartnerId);
                return (roundResult, bets);
            }

            return (new BandarQiuQiuRoundEntity(), Enumerable.Empty<BandarQiuQiuBetEntity>());
        }

        public async Task<(BandarQiuQiuRoundEntity, IEnumerable<BandarQiuQiuBetLogEntity>, Dictionary<string, decimal> Odds)> GetBetHistoryAsync(BetHistoryFilter filter)
        {
            var roundResult = await database.GetCollection<BandarQiuQiuRoundEntity>(Document.GetCollectionName(typeof(BandarQiuQiuRoundEntity)))
                .Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();
            var bet = await database.GetCollection<BandarQiuQiuBetEntity>(Document.GetCollectionName(typeof(BandarQiuQiuBetEntity)))
                .Find(x => x.Round == filter.RoundId && x.Uid == filter.UserId && x.OperatorId == filter.PartnerId).FirstOrDefaultAsync();

            var betLogs = await database.GetCollection<BandarQiuQiuBetLogEntity>(Document.GetCollectionName(typeof(BandarQiuQiuBetLogEntity)))
                .Find(x => x.Round == filter.RoundId && x.Uid == filter.UserId && x.OperatorId == filter.PartnerId).SortByDescending(x => x.Id).ToListAsync();

            return (roundResult, betLogs, bet.Odds);
        }
    }
}