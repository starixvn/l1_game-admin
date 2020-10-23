using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IBlackjackReportRepository
    {
        Task<IEnumerable<BlackjackBetReportEntity>> GetAsync(long handId, string nickname, bool excludeUUS);
    }

    public class BlackjackReportRepository : IBlackjackReportRepository
    {
        private readonly IMongoDatabase database;

        public BlackjackReportRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<BlackjackBetReportEntity>> GetAsync(long handId, string nickname, bool excludeUUS)
        {
            var project = GetAggregateBuilder(handId, nickname, excludeUUS)
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
                    Actions = x.Actions
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
            }

            return records;
        }

        private IAggregateFluent<BlackjackBetWithUnWindAccountsEntity> GetAggregateBuilder(long handId, string nickname, bool excludeUUS)
        {
            var betCollection = database.GetCollection<BlackjackBetEntity>(Document.GetCollectionName(typeof(BlackjackBetEntity)));
            var accountCollection = database.GetCollection<AccountEntity>(Document.GetCollectionName(typeof(AccountEntity)));
            var accountInfoCollection = database.GetCollection<AccountInfoEntity>(Document.GetCollectionName(typeof(AccountInfoEntity)));
            var nickNameRegex = new Regex($"(?i){nickname}");

            var aggregateBuilder = betCollection.Aggregate().Match(x => x.HandId == handId || handId == 0)
                .SortBy(x => x.Id)
                .Lookup<BlackjackBetEntity, AccountInfoEntity, BetEntityWithAccountInfos>(
                       foreignCollection: accountInfoCollection,
                       localField: x => x.Uid,
                       foreignField: x => x.UserId,
                       @as: r => r.AccountInfos)
                 .Unwind<BetEntityWithAccountInfos, BetEntityWithAccountInfoUnwind>(x => x.AccountInfos)
                 .Match(x => (string.IsNullOrWhiteSpace(nickname) || nickNameRegex.IsMatch(x.AccountInfos.Name)) && (!excludeUUS || x.AccountInfos.Currency != "UUS"))
                 .AppendStage<BetEntityWithAccountInfoUnwind>("{$addFields: {userId: {$toObjectId: '$uid'}}}")
                 .Lookup<BetEntityWithAccountInfoUnwind, AccountEntity, BetEntityWithAccountInfoUnwind>(
                       foreignCollection: accountCollection,
                       localField: x => x.UserId,
                       foreignField: x => x.Id,
                       @as: r => r.Accounts
                 )
                .Unwind<BetEntityWithAccountInfoUnwind, BlackjackBetWithUnWindAccountsEntity>(x => x.Accounts);

            return aggregateBuilder;
        }
    }
}