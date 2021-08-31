using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IHeroesSlotReportRepository
    {
        Task<IEnumerable<SlotGameBetReportEntity>> GetAsync(long roundId, string nickname, bool excludeUUS);
    }

    public class HeroesSlotReportRepository : IHeroesSlotReportRepository
    {
        private readonly IMongoDatabase database;

        public HeroesSlotReportRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<SlotGameBetReportEntity>> GetAsync(long roundId, string nickname, bool excludeUUS)
        {
            var project = GetAggregateBuilder(roundId, nickname, excludeUUS: excludeUUS)
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
                    Paid = x.Paid
                });

            return await project.ToListAsync();
        }

        private IAggregateFluent<SlotGameBetWithUnWindAccountsEntity> GetAggregateBuilder(long roundId, string nickname, bool excludeUUS)
        {
            var betCollection = database.GetCollection<HeroesSlotRoundEntity>(Document.GetCollectionName(typeof(HeroesSlotRoundEntity)));
            var accountCollection = database.GetCollection<AccountEntity>(Document.GetCollectionName(typeof(AccountEntity)));
            var accountInfoCollection = database.GetCollection<AccountInfoEntity>(Document.GetCollectionName(typeof(AccountInfoEntity)));
            var nickNameRegex = new Regex($"^(?i){nickname}$");

            var aggregateBuilder = betCollection.Aggregate()
                .Match(x => (x.Number == roundId || roundId == 0) && (string.IsNullOrWhiteSpace(nickname) || nickNameRegex.IsMatch(x.Name)))
                .SortByDescending(x => x.Id)
                .Lookup<HeroesSlotRoundEntity, AccountInfoEntity, BetEntityWithAccountInfos>(
                       foreignCollection: accountInfoCollection,
                       localField: x => x.Uid,
                       foreignField: x => x.UserId,
                       @as: r => r.AccountInfos)
                 .Unwind<BetEntityWithAccountInfos, BetEntityWithAccountInfoUnwind>(x => x.AccountInfos)
                 .Match(x => !excludeUUS || x.AccountInfos.Currency != "UUS")
                 .AppendStage<BetEntityWithAccountInfoUnwind>("{$addFields: {userId: {$toObjectId: '$uid'}}}")
                 .Lookup<BetEntityWithAccountInfoUnwind, AccountEntity, BetEntityWithAccountInfoUnwind>(
                       foreignCollection: accountCollection,
                       localField: x => x.UserId,
                       foreignField: x => x.Id,
                       @as: r => r.Accounts
                 )
                .Unwind<BetEntityWithAccountInfoUnwind, SlotGameBetWithUnWindAccountsEntity>(x => x.Accounts);

            return aggregateBuilder;
        }
    }
}