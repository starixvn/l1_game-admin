using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GamesAdmin.Core.Extensions;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IBlackjackReportRepository
    {
        Task<IEnumerable<BlackjackBetReportEntity>> GetAsync(BlackjackBetReportFilter filter);
    }

    public class BlackjackReportRepository : IBlackjackReportRepository
    {
        private readonly IMongoDatabase database;

        public BlackjackReportRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<BlackjackBetReportEntity>> GetAsync(BlackjackBetReportFilter filter)
        {
            var project = GetAggregateBuilder(filter)
                .Project(x => new BlackjackBetReportEntity
                {
                    Time = x.Time,
                    Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                    Nickname = x.AccountInfos.Name,
                    BetWin = x.Betwin,
                    Bet = x.Bet,
                    Cards = x.Cards,
                    SplitForm = x.SplitFrom,
                    Round = x.Round,
                    HandId = x.HandId,
                    Point = x.Total,
                    Actions = x.Actions,
                    SoftPoints = x.SoftPoints,
                    Currency = x.Currency,
                    PartnerId = x.PartnerId,
                    Rate = x.Rate
                });

            var records = await project.ToListAsync();
            var roundIds = records.Select(x => x.Round).ToList();

            var rounds = await database.GetCollection<BlackjackRoundEntity>(Document.GetCollectionName(typeof(BlackjackRoundEntity))).Find(x => roundIds.Contains(x.Number)).ToListAsync();

            foreach (var record in records)
            {
                var round = rounds.FirstOrDefault(x => x.Number == record.Round);
                record.DealerCards = round.Result;
                record.Ended = round.Ended;
                record.DealerPoint = round.SettlementResult;
                record.ExchangeRateId = round.ExchangeRateId;
            }

            return records;
        }

        private IAggregateFluent<BlackjackBetWithUnWindAccountsEntity> GetAggregateBuilder(BlackjackBetReportFilter filter)
        {
            var betCollection = database.GetCollection<BlackjackBetEntity>(Document.GetCollectionName(typeof(BlackjackBetEntity)));
            var accountCollection = database.GetCollection<AccountEntity>(Document.GetCollectionName(typeof(AccountEntity)));
            var accountInfoCollection = database.GetCollection<AccountInfoEntity>(Document.GetCollectionName(typeof(AccountInfoEntity)));
            var userNameRegex = new Regex($"^(?i){filter.Username}$");

            Expression<Func<BlackjackBetEntity, bool>> expression;

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
                expression = expression.And(x => x.PartnerId == filter.PartnerId);
            }

            if (!string.IsNullOrWhiteSpace(filter.Nickname))
            {
                var nickNameRegex = new Regex($"^(?i){filter.Nickname}$");
                expression = expression.And(x => nickNameRegex.IsMatch(x.Name));
            }

            var aggregateBuilder = betCollection.Aggregate()
                .Match(expression)
                .SortByDescending(x => x.Id)
                .Lookup<BlackjackBetEntity, AccountInfoEntity, BetEntityWithAccountInfos>(
                       foreignCollection: accountInfoCollection,
                       localField: x => x.Uid,
                       foreignField: x => x.UserId,
                       @as: r => r.AccountInfos)
                 .Unwind<BetEntityWithAccountInfos, BetEntityWithAccountInfoUnwind>(x => x.AccountInfos)
                 .Match(x => (!filter.ExcludeUUS || x.AccountInfos.Currency != "UUS"))
                 .AppendStage<BetEntityWithAccountInfoUnwind>("{$addFields: {userId: {$toObjectId: '$uid'}}}")
                 .Lookup<BetEntityWithAccountInfoUnwind, AccountEntity, BetEntityWithAccountInfoUnwind>(
                       foreignCollection: accountCollection,
                       localField: x => x.UserId,
                       foreignField: x => x.Id,
                       @as: r => r.Accounts
                 )
                .Unwind<BetEntityWithAccountInfoUnwind, BlackjackBetWithUnWindAccountsEntity>(x => x.Accounts)
                .Match(x => (string.IsNullOrWhiteSpace(filter.Username) || userNameRegex.IsMatch(x.Accounts.Local.MemberName)));

            return aggregateBuilder;
        }
    }
}