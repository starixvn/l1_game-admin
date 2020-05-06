using System;
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
            try
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
            catch (Exception e)
            {
                return new List<BigSmallBetHistoryReportEntity>();
            }
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

        //private async Task<IList<BetHistoryReportEntity>> GetByRoundFishPrawnCrab(long roundId, string name, byte gameType, DateTime fromDate, DateTime toDate)
        //{
        //    var roundCollection = database.GetCollection<FishPrawnCrabRoundEntity>(Document.GetCollectionName(typeof(FishPrawnCrabRoundEntity)));
        //    var betCollection = database.GetCollection<FishPrawnCrabBetEntity>(Document.GetCollectionName(typeof(FishPrawnCrabBetEntity)));

        //    var aggregateBuilder = roundCollection.Aggregate()
        //        .Match(round => round.Time >= fromDate && round.Time < toDate && round.Number == roundId)
        //        .Lookup<FishPrawnCrabRoundEntity, FishPrawnCrabBetEntity, FishPrawnCrabRoundWithBetsEntity>(
        //            foreignCollection: betCollection,
        //            localField: round => round.Number,
        //            foreignField: bet => bet.Round,
        //            @as: r => r.Bet
        //        )
        //        .Unwind<FishPrawnCrabRoundWithBetsEntity, FishPrawnCrabRoundWithBetUnwindEntity>(x => x.Bet)
        //        .Match(x => x.Bet.Name == name && x.Bet.Paid);

        //    var unwindEntities = await aggregateBuilder.ToListAsync();
        //    var results = unwindEntities.Select(x => x.ToGameReport()).ToList();

        //    return results;
        //}

        //private async Task<IList<BetHistoryReportEntity>> GetByRoundShakeThePlate(long roundId, string name, byte gameType, DateTime fromDate, DateTime toDate)
        //{
        //    var roundCollection = database.GetCollection<ShakeThePlateRoundEntity>(Document.GetCollectionName(typeof(ShakeThePlateRoundEntity)));
        //    var betCollection = database.GetCollection<ShakeThePlateBetEntity>(Document.GetCollectionName(typeof(ShakeThePlateBetEntity)));

        //    var aggregateBuilder = roundCollection.Aggregate()
        //        .Match(round => round.Time >= fromDate && round.Time < toDate && round.Number == roundId)
        //        .Lookup<ShakeThePlateRoundEntity, ShakeThePlateBetEntity, ShakeThePlateRoundWithBetsEntity>(
        //            foreignCollection: betCollection,
        //            localField: round => round.Number,
        //            foreignField: bet => bet.Round,
        //            @as: r => r.Bet
        //        )
        //        .Unwind<ShakeThePlateRoundWithBetsEntity, ShakeThePlateRoundWithBetUnwindEntity>(x => x.Bet)
        //        .Match(x => x.Bet.Name == name && x.Bet.Paid);

        //    var unwindEntities = await aggregateBuilder.ToListAsync();
        //    var results = unwindEntities.Select(x => x.ToGameReport()).ToList();

        //    return results;
        //}
    }
}