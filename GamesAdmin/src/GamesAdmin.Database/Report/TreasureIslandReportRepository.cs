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
    public interface ITreasureIslandReportRepository
    {
        Task<IEnumerable<CascadingSlotBetReportEntity>> GetRecordAsync(CascadingSlotReportFilter filter);

        Task<CascadingSlotBetHistoryEntity> GetBetHistoryAsync(CascadingSlotReportFilter filter);
    }

    public class TreasureIslandReportRepository : ITreasureIslandReportRepository
    {
        private readonly IMongoDatabase database;

        public TreasureIslandReportRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<CascadingSlotBetHistoryEntity> GetBetHistoryAsync(CascadingSlotReportFilter filter)
        {
            var round = await database
                .GetCollection<TreasureIslandRoundEntity>(Document.GetCollectionName(typeof(TreasureIslandRoundEntity))).Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();

            var totalMultiplier = await database
                .GetCollection<TreasureIslandTotalMultiplierEntity>(Document.GetCollectionName(typeof(TreasureIslandTotalMultiplierEntity))).Find(x => x.Name == filter.Nickname).ToListAsync();

            if(totalMultiplier.Count > 0)
            {
                var multiplier = totalMultiplier.FirstOrDefault(e => e.Rounds.Any(x => x.RoundId == filter.RoundId));
                if(multiplier != null)
                {
                    return new CascadingSlotBetHistoryEntity(round, multiplier.Rounds);
                }
            }

            return new CascadingSlotBetHistoryEntity(round);
        }

        public async Task<IEnumerable<CascadingSlotBetReportEntity>> GetRecordAsync(CascadingSlotReportFilter filter)
        {

            var project = GetAggregateBuilder(filter)
                .Project(x => new CascadingSlotBetReportEntity
                {
                    Time = x.Time,
                    Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                    Nickname = x.AccountInfos.Name,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    TotalAmount = x.TotalAmount,
                    Number = x.Number,
                    UserId = x.Uid,
                    Paid = x.Paid,
                    Currency = x.Currency,
                    OperatorId = x.OperatorId,
                    Rate = x.Rate,
                    ExchangeRateId = x.ExchangeRateId,
                });

            return await project.ToListAsync();
        }

        private IAggregateFluent<CascadingSlotBetWithUnWindAccountsEntity> GetAggregateBuilder(CascadingSlotReportFilter filter)
        {
            var betCollection = database.GetCollection<TreasureIslandRoundEntity>(Document.GetCollectionName(typeof(TreasureIslandRoundEntity)));
            var accountCollection = database.GetCollection<AccountEntity>(Document.GetCollectionName(typeof(AccountEntity)));
            var accountInfoCollection = database.GetCollection<AccountInfoEntity>(Document.GetCollectionName(typeof(AccountInfoEntity)));
            var expression = GetFilterExpression(filter);
            var userNameRegex = new Regex($"^(?i){filter.Username}$");

            var aggregateBuilder = betCollection.Aggregate()
                .Match(expression)
                .SortByDescending(x => x.Id)
                .Lookup<TreasureIslandRoundEntity, AccountInfoEntity, BetEntityWithAccountInfos>(
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
                .Unwind<BetEntityWithAccountInfoUnwind, CascadingSlotBetWithUnWindAccountsEntity>(x => x.Accounts)
                .Match(x => (string.IsNullOrWhiteSpace(filter.Username) || userNameRegex.IsMatch(x.Accounts.Local.MemberName)));


            return aggregateBuilder;
        }

        private Expression<Func<TreasureIslandRoundEntity, bool>> GetFilterExpression(CascadingSlotReportFilter filter)
        {
            Expression<Func<TreasureIslandRoundEntity, bool>> expression;
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