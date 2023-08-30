using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Database.Report
{
    public interface IColorGameReportRepository
    {
        Task<(ColorGameRoundEntity, IEnumerable<ColorGameBetReportEntity>)> GetRecordAsync(BetReportFilter filter);

        Task<(ColorGameRoundEntity, IEnumerable<ColorGameBetLogEntity>, Dictionary<string, decimal> Odds)> GetBetHistoryAsync(BetHistoryFilter filter);
    }

    public class ColorGameReportRepository : BaseBetReportRepository, IColorGameReportRepository
    {
        public ColorGameReportRepository(IMongoDatabase database) : base(database)
        {
        } 

        public async Task<(ColorGameRoundEntity, IEnumerable<ColorGameBetReportEntity>)> GetRecordAsync(BetReportFilter filter)
        {
            var roundResult = await database.GetCollection<ColorGameRoundEntity>(Document.GetCollectionName(typeof(ColorGameRoundEntity)))
                .Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<ColorGameBetEntity, ColorGameBetWithUnWindAccountsEntity>(
                        filter.RoundId, filter.Nickname, filter.ExcludeBot, filter.ExcludeUUS, filter.PartnerId, filter.Username)
                    .Project(x => new ColorGameBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        UserId = x.Uid,
                        Nickname = x.AccountInfos.Name,
                        Betwin = x.Betwin,
                        Yellow = x.Yellow,
                        Red = x.Red,
                        Pink = x.Pink,
                        White = x.White,
                        Blue = x.Blue,
                        Green = x.Green,                        
                        TotalAmount = x.TotalAmount,
                        Currency = x.Currency,
                        Rate = x.Rate,
                        OperatorId = x.OperatorId
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new ColorGameRoundEntity(), Enumerable.Empty<ColorGameBetReportEntity>());
        }

        public async Task<(ColorGameRoundEntity, IEnumerable<ColorGameBetLogEntity>, Dictionary<string, decimal> Odds)> GetBetHistoryAsync(BetHistoryFilter filter)
        {
            var roundResult = await database.GetCollection<ColorGameRoundEntity>(Document.GetCollectionName(typeof(ColorGameRoundEntity)))
                .Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();
            var bet = await database.GetCollection<ColorGameBetEntity>(Document.GetCollectionName(typeof(ColorGameBetEntity)))
                .Find(x => x.Round == filter.RoundId && x.Uid == filter.UserId && x.OperatorId == filter.PartnerId).FirstOrDefaultAsync();

            var betLogs = await database.GetCollection<ColorGameBetLogEntity>(Document.GetCollectionName(typeof(ColorGameBetLogEntity)))
                .Find(x => x.Round == filter.RoundId && x.Uid == filter.UserId && x.OperatorId == filter.PartnerId).SortByDescending(x => x.Id).ToListAsync();

            return (roundResult, betLogs, bet.Odds);
        }
    }
}