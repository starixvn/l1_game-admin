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
    public interface ICasinoHoldemPokerReportRepository
    {
        Task<IEnumerable<CasinoHoldemPokerBetReportEntity>> GetAsync(CasinoHoldemPokerBetReportFilter filter);
    }

    public class CasinoHoldemPokerReportRepository : BaseBetReportRepository, ICasinoHoldemPokerReportRepository
    {
        public CasinoHoldemPokerReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<IEnumerable<CasinoHoldemPokerBetReportEntity>> GetAsync(CasinoHoldemPokerBetReportFilter filter)
        {
            var projects = GetAggregateBuilder(filter).ToList();
            var roundIds = projects.Select(x => x.Round).ToList();
            var rounds = await database.GetCollection<CasinoHoldemPokerRoundEntity>(Document.GetCollectionName(typeof(CasinoHoldemPokerRoundEntity))).Find(x => roundIds.Contains(x.Number)).ToListAsync();

            var records = projects.Select(
               project => new CasinoHoldemPokerBetReportEntity(
                project.Time,
                project.PartnerId,
                project.AccountInfos.IsBot ? project.Accounts.Local.Username : project.Accounts.Local.MemberName,
                project.AccountInfos.Name,
                project.Currency,
                project,
                rounds.FirstOrDefault(round => round.Number == project.Round)));

            return records;
        }

        private IAggregateFluent<CasinoHoldemPokerBetWithUnWindAccountsEntity> GetAggregateBuilder(CasinoHoldemPokerBetReportFilter filter)
        {
            var betCollection = database.GetCollection<CasinoHoldemPokerBetEntity>(Document.GetCollectionName(typeof(CasinoHoldemPokerBetEntity)));
            var accountCollection = database.GetCollection<AccountEntity>(Document.GetCollectionName(typeof(AccountEntity)));
            var accountInfoCollection = database.GetCollection<AccountInfoEntity>(Document.GetCollectionName(typeof(AccountInfoEntity)));
            var userNameRegex = new Regex($"^(?i){filter.Username}$");

            Expression<Func<CasinoHoldemPokerBetEntity, bool>> expression;

            expression = x => x.Time <= filter.ToDate && x.Time >= filter.FromDate;

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
                .Lookup<CasinoHoldemPokerBetEntity, AccountInfoEntity, BetEntityWithAccountInfos>(
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
                .Unwind<BetEntityWithAccountInfoUnwind, CasinoHoldemPokerBetWithUnWindAccountsEntity>(x => x.Accounts)
                .Match(x => (string.IsNullOrWhiteSpace(filter.Username) || userNameRegex.IsMatch(x.Accounts.Local.MemberName)));

            return aggregateBuilder;
        }
    }
}