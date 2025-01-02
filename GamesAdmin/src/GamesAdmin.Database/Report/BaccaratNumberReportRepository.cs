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
    public interface IBaccaratNumberReportRepository
    {
        Task<(TRound, IEnumerable<TBet>)> GetRecordAsync<TRound, TBet>(BetReportFilter filter) 
            where TRound : BacBall1RoundEntity, new() 
            where TBet : BacBall1BetEntity;

        Task<(TRound, IEnumerable<TBetLog>, Dictionary<string, decimal> Odds)> GetBetHistoryAsync<TRound, TBet, TBetLog>(BetHistoryFilter filter) 
            where TRound : BacBall1RoundEntity
            where TBet : BacBall1BetEntity
            where TBetLog : BacBall1BetLogEntity;
    }

    public class BaccaratNumberReportRepository : BaseBetReportRepository, IBaccaratNumberReportRepository
    {
        public BaccaratNumberReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(TRound, IEnumerable<TBet>)> GetRecordAsync<TRound, TBet>(BetReportFilter filter)
            where TRound : BacBall1RoundEntity, new() 
            where TBet : BacBall1BetEntity
        {
            var roundFilter = Builders<TRound>.Filter.ElemMatch(x => x.Result, y => y.DrawId == filter.RoundId);
            var roundResult = await database.GetCollection<TRound>(Document.GetCollectionName(typeof(TRound)))
                .Find(roundFilter).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var bets = await GetBets<TBet>(filter.RoundId, filter.Nickname, filter.ExcludeUUS, filter.PartnerId);
                return (roundResult, bets);
            }

            return (new TRound(), Enumerable.Empty<TBet>());
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

        public async Task<(TRound, IEnumerable<TBetLog>, Dictionary<string, decimal> Odds)> GetBetHistoryAsync<TRound, TBet, TBetLog>(BetHistoryFilter filter)
            where TRound : BacBall1RoundEntity
            where TBet : BacBall1BetEntity
            where TBetLog : BacBall1BetLogEntity
        {
            var roundFilter = Builders<TRound>.Filter.ElemMatch(x => x.Result, y => y.DrawId == filter.RoundId);
            var roundResult = await database.GetCollection<TRound>(Document.GetCollectionName(typeof(TRound)))
                .Find(roundFilter).FirstOrDefaultAsync();
            var bet = await database.GetCollection<TBet>(Document.GetCollectionName(typeof(TBet)))
                .Find(x => x.DrawId == filter.RoundId && x.Uid == filter.UserId && x.OperatorId == filter.PartnerId).FirstOrDefaultAsync();

            var betLogs = await database.GetCollection<TBetLog>(Document.GetCollectionName(typeof(TBetLog)))
                    .Find(x => x.DrawId == filter.RoundId && x.Uid == filter.UserId && x.OperatorId == filter.PartnerId).SortByDescending(x => x.Time).ToListAsync();
           
            return (roundResult, betLogs, bet.Odds);
        }
    }
}