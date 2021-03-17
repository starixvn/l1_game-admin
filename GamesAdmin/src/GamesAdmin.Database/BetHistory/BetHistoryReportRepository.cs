using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IBetHistoryReportRepository
    {
        Task<IEnumerable<BigSmallBetHistoryReportEntity>> GetBigSmallAsync(long roundId, string name);

        Task<IEnumerable<BigSmallBetHistoryReportEntity>> GetBigSmallTurboAsync(long roundId, string name);

        Task<IEnumerable<OddEvenBetHistoryReportEntity>> GetOddEvenAsync(long roundId, string name);

        Task<IEnumerable<OddEvenBetHistoryReportEntity>> GetOddEvenTurboAsync(long roundId, string name);

        Task<(RouletteRoundEntity, IEnumerable<RouletteBetEntity>)> GetRouletteAsync(long roundId, string userId);

        Task<(SpecialSeDieRoundEntity, IEnumerable<SpecialSeDieBetEntity>)> GetSpecialSeDieAsync(long roundId, string userId);

        Task<(SicBoRoundEntity, IEnumerable<SicBoBetEntity>)> GetSicBoAsync(long roundId, string userId);

        Task<(VipSeDieWarRoundEntity, IEnumerable<VipSeDieWarBetEntity>)> GetVipSeDieWarAsync(long roundId, string userId);
    }

    public class BetHistoryReportRepository : IBetHistoryReportRepository
    {
        private readonly IMongoDatabase database;

        public BetHistoryReportRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<BigSmallBetHistoryReportEntity>> GetBigSmallAsync(long roundId, string name)
        {
            var roundCollection = database.GetCollection<BigSmallRoundEntity>(Document.GetCollectionName(typeof(BigSmallRoundEntity)));
            var oneCollection = database.GetCollection<BigSmallOnesEntity>(Document.GetCollectionName(typeof(BigSmallOnesEntity)));
            var accountInfoCollection = database.GetCollection<AccountInfoEntity>(Document.GetCollectionName(typeof(AccountInfoEntity)));

            var aggregateBuilder = roundCollection.Aggregate()
                .Match(x => x.Number == roundId || roundId == 0)
                .Lookup<BigSmallRoundEntity, BigSmallOnesEntity, BigSmallRoundWithOnesEntity>(
                    foreignCollection: oneCollection,
                    localField: round => round.Number,
                    foreignField: bet => bet.Round,
                    @as: r => r.One
                )
                .Unwind<BigSmallRoundWithOnesEntity, BigSmallRoundWithOneUnwindEntity>(x => x.One)
                .Match(x => x.One.Paid)
                .Lookup<BigSmallRoundWithOneUnwindEntity, AccountInfoEntity, BigSmallRoundWithOneUnwindEntity>(
                    foreignCollection: accountInfoCollection,
                    localField: round => round.One.Uid,
                    foreignField: account => account.UserId,
                    @as: r => r.Accounts
                )
                .Match(round => round.Accounts.ElementAt(0).Name == name);

            var unwindEntities = await aggregateBuilder.ToListAsync();

            var results = unwindEntities.Select(x =>
                new BigSmallBetHistoryReportEntity
                {
                    BetWin = x.One.Betwin,
                    Round = x.Number,
                    Time = x.One.Time,
                    Bet = x.One.Bet,
                    FirstDice = (byte)x.Dice1,
                    SecondDice = (byte)x.Dice2,
                    ThirdDice = (byte)x.Dice3,
                    Refund = x.One.Back,
                    Select = x.One.Select
                }
            );

            return results;
        }

        public async Task<IEnumerable<BigSmallBetHistoryReportEntity>> GetBigSmallTurboAsync(long roundId, string name)
        {
            var roundCollection = database.GetCollection<BigSmallTurboRoundEntity>(Document.GetCollectionName(typeof(BigSmallTurboRoundEntity)));
            var oneCollection = database.GetCollection<BigSmallTurboOnesEntity>(Document.GetCollectionName(typeof(BigSmallTurboOnesEntity)));
            var accountInfoCollection = database.GetCollection<AccountInfoEntity>(Document.GetCollectionName(typeof(AccountInfoEntity)));

            var aggregateBuilder = roundCollection.Aggregate()
                .Match(x => x.Number == roundId || roundId == 0)
                .Lookup<BigSmallTurboRoundEntity, BigSmallTurboOnesEntity, BigSmallTurboRoundWithOnesEntity>(
                    foreignCollection: oneCollection,
                    localField: round => round.Number,
                    foreignField: bet => bet.Round,
                    @as: r => r.One
                )
                .Unwind<BigSmallTurboRoundWithOnesEntity, BigSmallTurboRoundWithOneUnwindEntity>(x => x.One)
                .Match(x => x.One.Paid)
                .Lookup<BigSmallTurboRoundWithOneUnwindEntity, AccountInfoEntity, BigSmallTurboRoundWithOneUnwindEntity>(
                    foreignCollection: accountInfoCollection,
                    localField: round => round.One.Uid,
                    foreignField: account => account.UserId,
                    @as: r => r.Accounts
                )
                .Match(x => x.Accounts.ElementAt(0).Name == name);

            var unwindEntities = await aggregateBuilder.ToListAsync();
            var results = unwindEntities.Select(x =>
                new BigSmallBetHistoryReportEntity
                {
                    BetWin = x.One.Betwin,
                    Round = x.Number,
                    Time = x.One.Time,
                    Bet = x.One.Bet,
                    FirstDice = (byte)x.Dice1,
                    SecondDice = (byte)x.Dice2,
                    ThirdDice = (byte)x.Dice3,
                    Refund = x.One.Back,
                    Select = x.One.Select
                }
            );

            return results;
        }

        public async Task<IEnumerable<OddEvenBetHistoryReportEntity>> GetOddEvenAsync(long roundId, string name)
        {
            var roundCollection = database.GetCollection<OddEvenRoundEntity>(Document.GetCollectionName(typeof(OddEvenRoundEntity)));
            var oneCollection = database.GetCollection<OddEvenOnesEntity>(Document.GetCollectionName(typeof(OddEvenOnesEntity)));
            var accountInfoCollection = database.GetCollection<AccountInfoEntity>(Document.GetCollectionName(typeof(AccountInfoEntity)));

            var aggregateBuilder = roundCollection.Aggregate()
                   .Match(x => x.Number == roundId || roundId == 0)
                   .Lookup<OddEvenRoundEntity, OddEvenOnesEntity, OddEvenRoundWithOnesEntity>(
                       foreignCollection: oneCollection,
                       localField: round => round.Number,
                       foreignField: bet => bet.Round,
                       @as: r => r.One
                   )
                   .Unwind<OddEvenRoundWithOnesEntity, OddEvenRoundWithOneUnwindEntity>(x => x.One)
                   .Match(x => x.One.Paid)
                   .Lookup<OddEvenRoundWithOneUnwindEntity, AccountInfoEntity, OddEvenRoundWithOneUnwindEntity>(
                       foreignCollection: accountInfoCollection,
                       localField: round => round.One.Uid,
                       foreignField: account => account.UserId,
                       @as: r => r.Accounts
                   )
                   .Match(x => x.Accounts.ElementAt(0).Name == name);

            var unwindEntities = await aggregateBuilder.ToListAsync();

            var results = unwindEntities.Select(x =>
               new OddEvenBetHistoryReportEntity
               {
                   BetWin = x.One.Betwin,
                   Round = x.Number,
                   Time = x.One.Time,
                   Bet = x.One.Bet,
                   FirstDice = (byte)x.Dice1,
                   SecondDice = (byte)x.Dice2,
                   ThirdDice = (byte)x.Dice3,
                   Refund = x.One.Back,
                   Select = x.One.Select
               }
            );

            return results;
        }

        public async Task<IEnumerable<OddEvenBetHistoryReportEntity>> GetOddEvenTurboAsync(long roundId, string name)
        {
            var roundCollection = database.GetCollection<OddEvenTurboRoundEntity>(Document.GetCollectionName(typeof(OddEvenTurboRoundEntity)));
            var oneCollection = database.GetCollection<OddEvenTurboOnesEntity>(Document.GetCollectionName(typeof(OddEvenTurboOnesEntity)));
            var accountInfoCollection = database.GetCollection<AccountInfoEntity>(Document.GetCollectionName(typeof(AccountInfoEntity)));

            var aggregateBuilder = roundCollection.Aggregate()
                   .Match(x => x.Number == roundId || roundId == 0)
                   .Lookup<OddEvenTurboRoundEntity, OddEvenTurboOnesEntity, OddEvenTurboRoundWithOnesEntity>(
                       foreignCollection: oneCollection,
                       localField: round => round.Number,
                       foreignField: bet => bet.Round,
                       @as: r => r.One
                   )
                   .Unwind<OddEvenTurboRoundWithOnesEntity, OddEvenTurboRoundWithOneUnwindEntity>(x => x.One)
                   .Match(x => x.One.Paid)
                   .Lookup<OddEvenTurboRoundWithOneUnwindEntity, AccountInfoEntity, OddEvenTurboRoundWithOneUnwindEntity>(
                       foreignCollection: accountInfoCollection,
                       localField: round => round.One.Uid,
                       foreignField: account => account.UserId,
                       @as: r => r.Accounts
                   )
                   .Match(x => x.Accounts.ElementAt(0).Name == name);

            var unwindEntities = await aggregateBuilder.ToListAsync();

            var results = unwindEntities.Select(x =>
               new OddEvenBetHistoryReportEntity
               {
                   BetWin = x.One.Betwin,
                   Round = x.Number,
                   Time = x.One.Time,
                   Bet = x.One.Bet,
                   FirstDice = (byte)x.Dice1,
                   SecondDice = (byte)x.Dice2,
                   ThirdDice = (byte)x.Dice3,
                   Refund = x.One.Back,
                   Select = x.One.Select
               }
            );

            return results;
        }

        public async Task<(RouletteRoundEntity, IEnumerable<RouletteBetEntity>)> GetRouletteAsync(long roundId, string userId)
        {
            var roundCollection = database.GetCollection<RouletteRoundEntity>(Document.GetCollectionName(typeof(RouletteRoundEntity)));
            var roundResult = await roundCollection.Find(x => x.Number == roundId).FirstOrDefaultAsync();

            var betCollection = database.GetCollection<RouletteBetEntity>(Document.GetCollectionName(typeof(RouletteBetEntity)));
            var bets = await betCollection.Find(x => x.Round == roundId && x.Uid == userId).SortByDescending(x => x.Id).ToListAsync();

            return (roundResult, bets);
        }

        public async Task<(SpecialSeDieRoundEntity, IEnumerable<SpecialSeDieBetEntity>)> GetSpecialSeDieAsync(long roundId, string userId)
        {
            var roundCollection = database.GetCollection<SpecialSeDieRoundEntity>(Document.GetCollectionName(typeof(SpecialSeDieRoundEntity)));
            var roundResult = await roundCollection.Find(x => x.Number == roundId).FirstOrDefaultAsync();

            var betCollection = database.GetCollection<SpecialSeDieBetEntity>(Document.GetCollectionName(typeof(SpecialSeDieBetEntity)));
            var bets = await betCollection.Find(x => x.Round == roundId && x.Uid == userId).SortByDescending(x => x.Id).ToListAsync();

            return (roundResult, bets);
        }

        public async Task<(SicBoRoundEntity, IEnumerable<SicBoBetEntity>)> GetSicBoAsync(long roundId, string userId)
        {
            var roundCollection = database.GetCollection<SicBoRoundEntity>(Document.GetCollectionName(typeof(SicBoRoundEntity)));
            var roundResult = await roundCollection.Find(x => x.Number == roundId).FirstOrDefaultAsync();

            var betCollection = database.GetCollection<SicBoBetEntity>(Document.GetCollectionName(typeof(SicBoBetEntity)));
            var bets = await betCollection.Find(x => x.Round == roundId && x.Uid == userId).SortByDescending(x => x.Id).ToListAsync();

            return (roundResult, bets);
        }

        public async Task<(VipSeDieWarRoundEntity, IEnumerable<VipSeDieWarBetEntity>)> GetVipSeDieWarAsync(long roundId, string userId)
        {
            var roundCollection = database.GetCollection<VipSeDieWarRoundEntity>(Document.GetCollectionName(typeof(VipSeDieWarRoundEntity)));
            var roundResult = await roundCollection.Find(x => x.Number == roundId).FirstOrDefaultAsync();

            var betCollection = database.GetCollection<VipSeDieWarBetEntity>(Document.GetCollectionName(typeof(VipSeDieWarBetEntity)));
            var bets = await betCollection.Find(x => x.Round == roundId && x.Uid == userId).SortByDescending(x => x.Id).ToListAsync();

            return (roundResult, bets);
        }
    }
}