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
    public interface IBacBall2ReportRepository
    {
        Task<(BacBall2RoundEntity, IEnumerable<BacBall2BetEntity>)> GetRecordAsync(BetReportFilter filter);

        Task<(BacBall2RoundEntity, IEnumerable<BacBall2BetLogEntity>, Dictionary<string, decimal> Odds)> GetBetHistoryAsync(BetHistoryFilter filter);
    }

    public class BacBall2ReportRepository : BaseBetReportRepository, IBacBall2ReportRepository
    {
        public BacBall2ReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(BacBall2RoundEntity, IEnumerable<BacBall2BetEntity>)> GetRecordAsync(BetReportFilter filter)
        {
            var roundFilter = Builders<BacBall2RoundEntity>.Filter.ElemMatch(x => x.Result, y => y.DrawId == filter.RoundId);
            var roundResult = await database.GetCollection<BacBall2RoundEntity>(Document.GetCollectionName(typeof(BacBall2RoundEntity)))
                .Find(roundFilter).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var bets = await GetBets<BacBall2BetEntity>(filter.RoundId, filter.Nickname, filter.ExcludeUUS, filter.PartnerId);
                return (roundResult, bets);
            }

            return (new BacBall2RoundEntity(), Enumerable.Empty<BacBall2BetEntity>());
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

        public async Task<(BacBall2RoundEntity, IEnumerable<BacBall2BetLogEntity>, Dictionary<string, decimal> Odds)> GetBetHistoryAsync(BetHistoryFilter filter)
        {
            var roundFilter = Builders<BacBall2RoundEntity>.Filter.ElemMatch(x => x.Result, y => y.DrawId == filter.RoundId);
            var roundResult = await database.GetCollection<BacBall2RoundEntity>(Document.GetCollectionName(typeof(BacBall2RoundEntity)))
                .Find(roundFilter).FirstOrDefaultAsync();
            var bet = await database.GetCollection<BacBall2BetEntity>(Document.GetCollectionName(typeof(BacBall2BetEntity)))
                .Find(x => x.DrawId == filter.RoundId && x.Uid == filter.UserId && x.OperatorId == filter.PartnerId).FirstOrDefaultAsync();

            var betLogs = await database.GetCollection<BacBall2BetLogEntity>(Document.GetCollectionName(typeof(BacBall2BetLogEntity)))
                    .Find(x => x.DrawId == filter.RoundId && x.Uid == filter.UserId && x.OperatorId == filter.PartnerId).SortByDescending(x => x.Time).ToListAsync();
           
            return (roundResult, betLogs, bet.Odds);
        }
    }
}