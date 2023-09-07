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
    public interface IPontoonReportRepository
    {
        Task<IEnumerable<BankingGameBetReportEntity>> GetAsync(BankingGameBetReportFilter filter);
    }
    public class PontoonReportRepository : IPontoonReportRepository
    {
        private readonly IMongoDatabase database;

        public PontoonReportRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<BankingGameBetReportEntity>> GetAsync(BankingGameBetReportFilter filter)
        {
            var userNameRegex = new Regex($"^(?i){filter.Username}$");

            var betCollection = database.GetCollection<PontoonBetEntity>(Document.GetCollectionName(typeof(PontoonBetEntity)));
            var accountCollection = database.GetCollection<AccountEntity>(Document.GetCollectionName(typeof(AccountEntity)));

            var bets = await betCollection.Find(GetFilterExpression(filter)).ToListAsync();
            var roundIds = bets.Select(x => x.Round).ToList();
            var rounds = await database.GetCollection<PontoonRoundEntity>(Document.GetCollectionName(typeof(PontoonRoundEntity))).Find(x => roundIds.Contains(x.Number)).ToListAsync();
            var result = new List<BankingGameBetReportEntity>();
            foreach (var bet in bets)
            {
                var betUser = await accountCollection.Find(acount => acount.Id == ObjectId.Parse(bet.Uid)).FirstOrDefaultAsync();
                var round = rounds.FirstOrDefault(x => x.Number == bet.Round);
                if (string.IsNullOrWhiteSpace(filter.Username) || userNameRegex.IsMatch(betUser.Local.MemberName))
                {
                    result.Add(new BankingGameBetReportEntity
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

        private static Expression<Func<PontoonBetEntity, bool>> GetFilterExpression(BankingGameBetReportFilter filter)
        {
            var from = filter.FromDate.ToStartDate().ToGMT7();
            var to = filter.ToDate.ToEndDate().ToGMT7();

            Expression<Func<PontoonBetEntity, bool>> expression = x =>
                (filter.HandId > 0 && x.HandId == filter.HandId) ||
                (x.Time >= from && x.Time <= to) &&
                (string.IsNullOrWhiteSpace(filter.PartnerId) || x.OperatorId == filter.PartnerId) &&
                (string.IsNullOrWhiteSpace(filter.Nickname) || new Regex($"^(?i){filter.Nickname}$").IsMatch(x.Name)) &&
                (!filter.ExcludeUUS || x.Currency != "UUS");

            return expression;
        }
    }
}
