using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IFourGuardiansReportRepository
    {
        Task<IEnumerable<FourGuardiansBetReportEntity>> GetAsync(long roundId, string nickname, bool excludeUUS);
    }

    public class FourGuardiansReportRepository : IFourGuardiansReportRepository
    {
        private readonly IMongoDatabase database;

        public FourGuardiansReportRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<FourGuardiansBetReportEntity>> GetAsync(long roundId, string nickname, bool excludeUUS)
        {
            var project = GetAggregateBuilder(roundId, nickname, excludeUUS: excludeUUS)
                .Project(x => new FourGuardiansBetReportEntity
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
                });

            return await project.ToListAsync();
        }

        private IAggregateFluent<FourGuardiansBetWithUnWindAccountsEntity> GetAggregateBuilder(long roundId, string nickname, bool excludeUUS)
        {
            var betCollection = database.GetCollection<FourGuardiansRoundEntity>(Document.GetCollectionName(typeof(FourGuardiansRoundEntity)));
            var accountCollection = database.GetCollection<AccountEntity>(Document.GetCollectionName(typeof(AccountEntity)));
            var accountInfoCollection = database.GetCollection<AccountInfoEntity>(Document.GetCollectionName(typeof(AccountInfoEntity)));
            var nickNameRegex = new Regex($"^(?i){nickname}$");

            var aggregateBuilder = betCollection.Aggregate().Match(x => x.Number == roundId || roundId == 0)
                .SortByDescending(x => x.Id)
                .Lookup<FourGuardiansRoundEntity, AccountInfoEntity, BetEntityWithAccountInfos>(
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
                .Unwind<BetEntityWithAccountInfoUnwind, FourGuardiansBetWithUnWindAccountsEntity>(x => x.Accounts);

            return aggregateBuilder;
        }
    }
}