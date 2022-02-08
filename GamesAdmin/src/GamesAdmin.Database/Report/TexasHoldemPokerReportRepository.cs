using GamesAdmin.Core.Extensions;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GamesAdmin.Database
{
    public class TexasHoldemPokerReportRepository : BaseBetReportRepository, IHoldemPokerReportRepository
    {
        public TexasHoldemPokerReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<IEnumerable<HoldemPokerBetReportEntity>> GetAsync(HoldemPokerBetReportFilter filter)
        {
            var projects = GetAggregateBuilder(filter).ToList();
            var roundIds = projects.Select(x => x.Round).ToList();
            var rounds = await database.GetCollection<TexasHoldemPokerRoundEntity>(Document.GetCollectionName(typeof(TexasHoldemPokerRoundEntity))).Find(x => roundIds.Contains(x.Number)).ToListAsync();

            var records = projects.Select(
               project => new TexasHoldemPokerBetReportEntity(
                project.Time,
                project.PartnerId,
                project.AccountInfos.IsBot ? project.Accounts.Local.Username : project.Accounts.Local.MemberName,
                project.AccountInfos.Name,
                project.Currency,
                project,
                rounds.FirstOrDefault(round => round.Number == project.Round)));

            return records;
        }

        public async Task<HoldemPokerBetHistoryModel> GetBetHistoryAsync(HoldemPokerBetHistoryFilter filter)
        {
            var bet = await database
                .GetCollection<TexasHoldemPokerBetEntity>(Document.GetCollectionName(typeof(TexasHoldemPokerBetEntity))).Find(x => x.Round == filter.RoundId && x.Name == filter.Nickname).FirstOrDefaultAsync();

            var round = await database
                .GetCollection<TexasHoldemPokerRoundEntity>(Document.GetCollectionName(typeof(TexasHoldemPokerRoundEntity))).Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();

            return new TexasHoldemPokerBetHistoryModel(bet, round, filter.ConvertToUSD);
        }


        private IAggregateFluent<TexasHoldemPokerBetWithUnWindAccountsEntity> GetAggregateBuilder(HoldemPokerBetReportFilter filter)
        {
            var betCollection = database.GetCollection<TexasHoldemPokerBetEntity>(Document.GetCollectionName(typeof(TexasHoldemPokerBetEntity)));
            var accountCollection = database.GetCollection<AccountEntity>(Document.GetCollectionName(typeof(AccountEntity)));
            var accountInfoCollection = database.GetCollection<AccountInfoEntity>(Document.GetCollectionName(typeof(AccountInfoEntity)));
            var userNameRegex = new Regex($"^(?i){filter.Username}$");

            Expression<Func<TexasHoldemPokerBetEntity, bool>> expression;

            if (filter.RoundId > 0)
            {
                expression = x => x.Round == filter.RoundId;
            }
            else
            {
                var gapHourTimeZone = -7;
                var fromDate = filter.FromDate.AddHours(gapHourTimeZone).ToUniversalTime();
                var toDate = filter.ToDate.AddDays(1).AddHours(gapHourTimeZone).AddMilliseconds(-1).ToUniversalTime();
                expression = x => x.Time >= fromDate && x.Time <= toDate;
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
                .Lookup<TexasHoldemPokerBetEntity, AccountInfoEntity, BetEntityWithAccountInfos>(
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
                .Unwind<BetEntityWithAccountInfoUnwind, TexasHoldemPokerBetWithUnWindAccountsEntity>(x => x.Accounts)
                .Match(x => (string.IsNullOrWhiteSpace(filter.Username) || userNameRegex.IsMatch(x.Accounts.Local.MemberName)));

            return aggregateBuilder;
        }
    }
}
