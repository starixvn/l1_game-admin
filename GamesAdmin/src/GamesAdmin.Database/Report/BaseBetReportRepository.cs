using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GamesAdmin.Core.Extensions;
using GamesAdmin.Database.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public abstract class BaseBetReportRepository
    {
        protected readonly IMongoDatabase database;

        protected BaseBetReportRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        protected IAggregateFluent<TResultEntity> GetBaseAggregateBuilder<TBetEntity, TResultEntity>(
            long roundId,
            string nickname,
            bool excludeBot = false,
            bool excludeUUS = false,
            string partnerId = null,
            string username = null)
                where TBetEntity : BaseBetEntity where TResultEntity : BaseBetEntity
        {
            var betCollection = database.GetCollection<TBetEntity>(Document.GetCollectionName(typeof(TBetEntity)));
            var accountCollection = database.GetCollection<AccountEntity>(Document.GetCollectionName(typeof(AccountEntity)));
            var accountInfoCollection = database.GetCollection<AccountInfoEntity>(Document.GetCollectionName(typeof(AccountInfoEntity)));
            var userNameRegex = new Regex($"(?i){username}");
            var expression = GetExpression<TBetEntity>(roundId, nickname, excludeUUS, partnerId);

            var aggregateBuilder = betCollection.Aggregate().Match(expression)
                .SortBy(x => x.Id)
                .Lookup<TBetEntity, AccountInfoEntity, BetEntityWithAccountInfos>(
                       foreignCollection: accountInfoCollection,
                       localField: x => x.Uid,
                       foreignField: x => x.UserId,
                       @as: r => r.AccountInfos)
                 .Unwind<BetEntityWithAccountInfos, BetEntityWithAccountInfoUnwind>(x => x.AccountInfos)
                 .Match(x => (!excludeBot || !x.AccountInfos.IsBot))
                 .AppendStage<BetEntityWithAccountInfoUnwind>("{$addFields: {userId: {$toObjectId: '$uid'}}}")
                 .Lookup<BetEntityWithAccountInfoUnwind, AccountEntity, BetEntityWithAccountInfoUnwind>(
                       foreignCollection: accountCollection,
                       localField: x => x.UserId,
                       foreignField: x => x.Id,
                       @as: r => r.Accounts
                 )
                .Match(x => username == null || x.Accounts.Any(account => userNameRegex.IsMatch(account.Local.MemberName)))
                .Unwind<BetEntityWithAccountInfoUnwind, TResultEntity>(x => x.Accounts);

            return aggregateBuilder;
        }

        protected async Task<IEnumerable<TBetEntity>> GetBets<TBetEntity>(
            long roundId,
            string nickname,
            bool excludeUUS = false,
            string partnerId = null)
            where TBetEntity : BaseBetEntity
        {
            var betCollection = database.GetCollection<TBetEntity>(Document.GetCollectionName(typeof(TBetEntity)));
            var expression = GetExpression<TBetEntity>(roundId, nickname, excludeUUS, partnerId);

            return await betCollection.Find(expression).SortBy(x => x.Id).ToListAsync();
        }

        protected virtual Expression<Func<TBetEntity, bool>> GetExpression<TBetEntity>(
            long roundId,
            string nickname,
            bool excludeUUS = false,
            string partnerId = null) where TBetEntity : BaseBetEntity
        {
            Expression<Func<TBetEntity, bool>> expression;
            var nickNameRegex = new Regex($"(?i){nickname}");

            if (string.IsNullOrWhiteSpace(nickname))
            {
                expression = x => x.Round == roundId;
            }
            else
            {
                expression = x => x.Round == roundId && nickNameRegex.IsMatch(x.Name);
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
    }

    internal class BetEntityWithAccountInfoUnwind : BaseBetEntity
    {
        public ObjectId UserId { get; set; }

        public AccountInfoEntity AccountInfos { get; set; }

        public IEnumerable<AccountEntity> Accounts { get; set; }
    }

    internal class BetEntityWithAccountInfos : BaseBetEntity
    {
        public IEnumerable<AccountInfoEntity> AccountInfos { get; set; }
    }
}