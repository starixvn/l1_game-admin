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
        Task<(BandarQiuQiuRoundEntity, IEnumerable<BandarQiuQiuBetReportEntity>)> GetRecordAsync(BetReportFilter filter);

        Task<(BandarQiuQiuRoundEntity, IEnumerable<BandarQiuQiuBetLogEntity>, Dictionary<string, decimal> Odds)> GetBetHistoryAsync(BetHistoryFilter filter);
    }

    public class BandarQiuQiuReportRepository : BaseBetReportRepository, IBandarQiuQiuReportRepository
    {
        public BandarQiuQiuReportRepository(IMongoDatabase database) : base(database)
        {
        } 

        public async Task<(BandarQiuQiuRoundEntity, IEnumerable<BandarQiuQiuBetReportEntity>)> GetRecordAsync(BetReportFilter filter)
        {
            var roundResult = await database.GetCollection<BandarQiuQiuRoundEntity>(Document.GetCollectionName(typeof(BandarQiuQiuRoundEntity)))
                .Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<BandarQiuQiuBetEntity, BandarQiuQiuBetWithUnWindAccountsEntity>(
                        filter.RoundId, filter.Nickname, filter.ExcludeBot, filter.ExcludeUUS, filter.PartnerId, filter.Username)
                    .Project(x => new BandarQiuQiuBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        UserId = x.Uid,
                        Nickname = x.AccountInfos.Name,
                        Betwin = x.Betwin,
                        Spade = x.Spade,
                        Heart = x.Heart,
                        Club = x.Club,
                        Diamond = x.Diamond,                       
                        TotalAmount = x.TotalAmount,
                        Currency = x.Currency,
                        Rate = x.Rate,
                        OperatorId = x.OperatorId
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new BandarQiuQiuRoundEntity(), Enumerable.Empty<BandarQiuQiuBetReportEntity>());
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