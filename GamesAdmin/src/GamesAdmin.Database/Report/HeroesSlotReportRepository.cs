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
    public interface IHeroesSlotReportRepository
    {
        Task<IEnumerable<SlotGameBetReportEntity>> GetAsync(SlotGameBetReportFilter filter);
    }

    public class HeroesSlotReportRepository : IHeroesSlotReportRepository
    {
        private readonly IMongoDatabase database;

        public HeroesSlotReportRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<SlotGameBetReportEntity>> GetAsync(SlotGameBetReportFilter filter)
        {
            var project = GetAggregateBuilder(filter)
                .Project(x => new SlotGameBetReportEntity
                {
                    Time = x.Time,
                    Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                    Nickname = x.AccountInfos.Name,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    TotalAmount = x.TotalAmount,
                    BonusChests = x.BonusChests,
                    FreeSpin = x.FreeSpin,
                    Number = x.Number,
                    Paylines = x.Paylines,
                    SettlementResult = x.SettlementResult,
                    Result = x.Result,
                    UserId = x.Uid,
                    Paid = x.Paid,
                    Currency = x.Currency,
                    OperatorId = x.OperatorId,
                    Rate = x.Rate,
                    ExchangeRateId = x.ExchangeRateId
                });

            return await project.ToListAsync();
        }

        private IAggregateFluent<SlotGameBetWithUnWindAccountsEntity> GetAggregateBuilder(SlotGameBetReportFilter filter)
        {
            var betCollection = database.GetCollection<HeroesSlotRoundEntity>(Document.GetCollectionName(typeof(HeroesSlotRoundEntity)));
            var accountCollection = database.GetCollection<AccountEntity>(Document.GetCollectionName(typeof(AccountEntity)));
            var accountInfoCollection = database.GetCollection<AccountInfoEntity>(Document.GetCollectionName(typeof(AccountInfoEntity)));
            var expression = GetFilterExpression(filter);
            var userNameRegex = new Regex($"^(?i){filter.Username}$");

            var aggregateBuilder = betCollection.Aggregate()
                .Match(expression)
                .SortByDescending(x => x.Id)
                .Lookup<HeroesSlotRoundEntity, AccountInfoEntity, BetEntityWithAccountInfos>(
                       foreignCollection: accountInfoCollection,
                       localField: x => x.Uid,
                       foreignField: x => x.UserId,
                       @as: r => r.AccountInfos)
                 .Unwind<BetEntityWithAccountInfos, BetEntityWithAccountInfoUnwind>(x => x.AccountInfos)
                 .Match(x => !filter.ExcludeUUS || x.AccountInfos.Currency != "UUS")
                 .AppendStage<BetEntityWithAccountInfoUnwind>("{$addFields: {userId: {$toObjectId: '$uid'}}}")
                 .Lookup<BetEntityWithAccountInfoUnwind, AccountEntity, BetEntityWithAccountInfoUnwind>(
                       foreignCollection: accountCollection,
                       localField: x => x.UserId,
                       foreignField: x => x.Id,
                       @as: r => r.Accounts
                 )
                .Unwind<BetEntityWithAccountInfoUnwind, SlotGameBetWithUnWindAccountsEntity>(x => x.Accounts)
                .Match(x => (string.IsNullOrWhiteSpace(filter.Username) || userNameRegex.IsMatch(x.Accounts.Local.MemberName)));


            return aggregateBuilder;
        }

        private Expression<Func<HeroesSlotRoundEntity, bool>> GetFilterExpression(SlotGameBetReportFilter filter)
        {
            Expression<Func<HeroesSlotRoundEntity, bool>> expression;
            if (filter.RoundId > 0)
            {
                expression = x => x.Number == filter.RoundId;
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

            return expression;
        }
    }
}