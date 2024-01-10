using System;
using System.Collections.Generic;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Report.Entities;
using System.Threading.Tasks;
using GamesAdmin.Core.Extensions;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Linq;
using MongoDB.Bson;

namespace GamesAdmin.Database.Report
{
    public interface IChineseBlackjackReportRepository
    {
        Task<IEnumerable<BlackjackBetReportEntity>> GetAsync(BlackjackBetReportFilter filter);
    }
    public class ChineseBlackjackReportRepository : IChineseBlackjackReportRepository
    {
        private readonly IMongoDatabase database;

        public ChineseBlackjackReportRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<BlackjackBetReportEntity>> GetAsync(BlackjackBetReportFilter filter)
        {
            var userNameRegex = new Regex($"^(?i){filter.Username}$");

            var betCollection = database.GetCollection<ChineseBlackjackBetEntity>(Document.GetCollectionName(typeof(ChineseBlackjackBetEntity)));
            var accountCollection = database.GetCollection<AccountEntity>(Document.GetCollectionName(typeof(AccountEntity)));

            var bets = await betCollection.Find(GetFilterExpression(filter)).SortByDescending(x => x.Id).ToListAsync();
            var roundIds = bets.Select(x => x.Round).ToList();
            var rounds = await database.GetCollection<ChineseBlackjackRoundEntity>(Document.GetCollectionName(typeof(ChineseBlackjackRoundEntity))).Find(x => roundIds.Contains(x.Number)).ToListAsync();
            var result = new List<BlackjackBetReportEntity>();
            foreach (var bet in bets)
            {
                var betUser = await accountCollection.Find(acount => acount.Id == ObjectId.Parse(bet.Uid)).FirstOrDefaultAsync();
                var round = rounds.FirstOrDefault(x => x.Number == bet.Round);
                if (string.IsNullOrWhiteSpace(filter.Username) || userNameRegex.IsMatch(betUser.Local.MemberName))
                {
                    result.Add(new BlackjackBetReportEntity
                    {
                        Time = bet.Time,
                        Username = betUser.Local.MemberName ?? betUser.Local.Username,
                        Nickname = bet.Name,
                        BetWin = bet.Betwin,
                        Bet = bet.Bet,
                        Cards = bet.Cards,
                        SplitForm = bet.SplitFrom,
                        Round = bet.Round,
                        HandId = bet.HandId,
                        Point = bet.Total,
                        Actions = bet.Actions,
                        SoftPoints = bet.SoftPoints,
                        Currency = bet.Currency,
                        PartnerId = bet.OperatorId,
                        Rate = bet.Rate,
                        DealerCards = round.Result,
                        Ended = round.Ended,
                        DealerPoint = round.SettlementResult,
                        ExchangeRateId = round.ExchangeRateId
                    });
                }
            }

            return result;
        }

        private static Expression<Func<ChineseBlackjackBetEntity, bool>> GetFilterExpression(BlackjackBetReportFilter filter)
        {
            Expression<Func<ChineseBlackjackBetEntity, bool>> expression;
            if (filter.HandId > 0)
            {
                expression = x => x.HandId == filter.HandId;
            }
            else
            {
                var from = filter.FromDate.ToStartDate().ToGMT7();
                var to = filter.ToDate.ToEndDate().ToGMT7();
                expression = x => from <= x.Time && x.Time <= to;
            }

            if (!string.IsNullOrWhiteSpace(filter.PartnerId))
            {
                expression = expression.And(x => x.OperatorId == filter.PartnerId);
            }

            if (!string.IsNullOrWhiteSpace(filter.Nickname))
            {
                var nickNameRegex = new Regex($"^(?i){filter.Nickname}$");
                expression = expression.And(x => nickNameRegex.IsMatch(x.Name));
            }

            if (filter.ExcludeUUS)
            {
                expression = expression.And(x => x.Currency != "UUS");
            }

            return expression;
        }
    }
}
