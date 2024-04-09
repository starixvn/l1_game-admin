using GamesAdmin.Core.Extensions;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GamesAdmin.Database.Report
{
    public interface IBacBall1ReportRepository
    {
        Task<(BacBall1RoundEntity, IEnumerable<BacBall1BetEntity>)> GetRecordAsync(BetReportFilter filter);

        Task<(BacBall1RoundEntity, IEnumerable<BacBall1BetLogEntity>, Dictionary<string, decimal> Odds)> GetBetHistoryAsync(BetHistoryFilter filter);
    }

    public class BacBall1ReportRepository : BaseBetReportRepository, IBacBall1ReportRepository
    {
        public BacBall1ReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(BacBall1RoundEntity, IEnumerable<BacBall1BetEntity>)> GetRecordAsync(BetReportFilter filter)
        {
            var roundFilter = Builders<BacBall1RoundEntity>.Filter.ElemMatch(x => x.Result, y => y.DrawId == filter.RoundId);
            var roundResult = await database.GetCollection<BacBall1RoundEntity>(Document.GetCollectionName(typeof(BacBall1RoundEntity)))
                .Find(roundFilter).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var bets = await GetBets<BacBall1BetEntity>(filter.RoundId, filter.Nickname, filter.ExcludeUUS, filter.PartnerId);
                return (roundResult, bets);
            }

            return (new BacBall1RoundEntity(), Enumerable.Empty<BacBall1BetEntity>());
        }

        protected async Task<IEnumerable<TBetEntity>> GetBets<TBetEntity>(
            long roundId,
            string nickname,
            bool excludeUUS = false,
            string partnerId = null)
            where TBetEntity : BaseBetEntity
        {
            var betCollection = database.GetCollection<TBetEntity>(Document.GetCollectionName(typeof(TBetEntity)));

            Expression<Func<TBetEntity, bool>> expression;
            var nickNameRegex = new Regex($"(?i){nickname}");

            if (string.IsNullOrWhiteSpace(nickname))
            {
                expression = x => x.DrawId == roundId;
            }
            else
            {
                expression = x => x.DrawId == roundId && nickNameRegex.IsMatch(x.Name);
            }
            if (!string.IsNullOrEmpty(partnerId))
            {
                expression = expression.And(x => x.OperatorId == partnerId);
            }

            if (excludeUUS)
            {
                expression = expression.And(x => x.Currency != "UUS");
            }

            return await betCollection.Find(expression).SortBy(x => x.Id).ToListAsync();
        }        

        public async Task<(BacBall1RoundEntity, IEnumerable<BacBall1BetLogEntity>, Dictionary<string, decimal> Odds)> GetBetHistoryAsync(BetHistoryFilter filter)
        {
            var roundFilter = Builders<BacBall1RoundEntity>.Filter.ElemMatch(x => x.Result, y => y.DrawId == filter.RoundId);
            var roundResult = await database.GetCollection<BacBall1RoundEntity>(Document.GetCollectionName(typeof(BacBall1RoundEntity)))
                .Find(roundFilter).FirstOrDefaultAsync();
            var bet = await database.GetCollection<BacBall1BetEntity>(Document.GetCollectionName(typeof(BacBall1BetEntity)))
                .Find(x => x.DrawId == filter.RoundId && x.Uid == filter.UserId && x.OperatorId == filter.PartnerId).FirstOrDefaultAsync();

            var betLogs = await database.GetCollection<BacBall1BetLogEntity>(Document.GetCollectionName(typeof(BacBall1BetLogEntity)))
                    .Find(x => x.DrawId == filter.RoundId && x.Uid == filter.UserId && x.OperatorId == filter.PartnerId).SortByDescending(x => x.Time).ToListAsync();
           
            return (roundResult, betLogs, bet.Odds);
        }
    }
}