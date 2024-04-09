using GamesAdmin.Core.Extensions;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GamesAdmin.Database.Report
{
    public interface IBaseDrawGameReportRepository
    {
        Task<(TRound, IEnumerable<TBet>)> GetRecordAsync<TRound, TBet>(BetReportFilter filter)
             where TRound : BaseRoundEntity where TBet : BaseBetEntity;

        Task<(TRound, IEnumerable<TBetLog>, Dictionary<string, decimal> Odds)> GetBetHistoryAsync<TRound, TBet, TBetLog>(BetHistoryFilter filter)
             where TRound : BaseRoundEntity where TBet : BaseBetEntity where TBetLog : BaseBetLogEntity;
    }

    public class BaseDrawGameReportRepository : BaseBetReportRepository, IBaseDrawGameReportRepository
    {
        public BaseDrawGameReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(TRound, IEnumerable<TBet>)> GetRecordAsync<TRound, TBet>(BetReportFilter filter)
             where TRound : BaseRoundEntity where TBet : BaseBetEntity
        {
            var roundFilter = Builders<TRound>.Filter.Eq("Result.DrawId", filter.RoundId);

            var roundResult = await database.GetCollection<TRound>(Document.GetCollectionName(typeof(TRound)))
                .Find(roundFilter).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var bets = await GetBets<TBet>(filter.RoundId, filter.Nickname, filter.ExcludeUUS, filter.PartnerId);
                return (roundResult, bets);
            }

            return (default, Enumerable.Empty<TBet>());
        }

        protected override Expression<Func<TBetEntity, bool>> GetExpression<TBetEntity>(
            long roundId,
            string nickname,
            bool excludeUUS = false,
            string partnerId = null)
        {
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
            return expression;
        }

        public async Task<(TRound, IEnumerable<TBetLog>, Dictionary<string, decimal> Odds)> GetBetHistoryAsync<TRound, TBet, TBetLog>(BetHistoryFilter filter)
             where TRound : BaseRoundEntity where TBet : BaseBetEntity where TBetLog : BaseBetLogEntity
        {
            var roundFilter = Builders<TRound>.Filter.Eq("Result.DrawId", filter.RoundId);

            var roundResult = await database.GetCollection<TRound>(Document.GetCollectionName(typeof(TRound)))
                .Find(roundFilter).FirstOrDefaultAsync();

            var bet = await database.GetCollection<TBet>(Document.GetCollectionName(typeof(TBet)))
                .Find(x => x.DrawId == filter.RoundId && x.Uid == filter.UserId && x.OperatorId == filter.PartnerId).FirstOrDefaultAsync();

            var betLogs = await database.GetCollection<TBetLog>(Document.GetCollectionName(typeof(TBetLog)))
                    .Find(x => x.DrawId == filter.RoundId && x.Uid == filter.UserId && x.OperatorId == filter.PartnerId).SortByDescending(x => x.Time).ToListAsync();

            var odds = bet.GetType().GetProperty("Odds").GetValue(bet);
            return (roundResult, betLogs, JsonConvert.DeserializeObject<Dictionary<string, decimal>>(JsonConvert.SerializeObject(odds)));
        }
    }
}