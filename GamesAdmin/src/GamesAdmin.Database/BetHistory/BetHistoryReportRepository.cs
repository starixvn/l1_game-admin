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
        //Task<IList<BetHistoryReportEntity>> GetAll(string name, byte gameType, DateTime fromDate, DateTime toDate);

        //Task<IList<BetHistoryReportEntity>> GetByRound(long round, string name, byte gameType, DateTime fromDate, DateTime toDate);

        Task<IEnumerable<BigSmallBetHistoryReportEntity>> GetBigSmallAsync(long roundId, string name);

        Task<IEnumerable<BigSmallBetHistoryReportEntity>> GetBigSmallTurboAsync(long roundId, string name);
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
                .Unwind<BigSmallRoundWithOnesEntity, BigSmallRoundWithBetUnwindEntity>(x => x.One)
                .Match(x => x.One.Paid)
                .Lookup<BigSmallRoundWithBetUnwindEntity, AccountInfoEntity, BigSmallRoundWithBetUnwindEntity>(
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
                    Time = x.Time,
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
                .Lookup<BigSmallTurboRoundEntity, BigSmallTurboOnesEntity, BigSmallTurboRoundWithBetsEntity>(
                    foreignCollection: oneCollection,
                    localField: round => round.Number,
                    foreignField: bet => bet.Round,
                    @as: r => r.One
                )
                .Unwind<BigSmallTurboRoundWithBetsEntity, BigSmallTurboRoundWithBetUnwindEntity>(x => x.One)
                .Match(x => x.One.Paid)
                .Lookup<BigSmallTurboRoundWithBetUnwindEntity, AccountInfoEntity, BigSmallTurboRoundWithBetUnwindEntity>(
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
                    Time = x.Time,
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

        private async Task<IList<BetHistoryReportEntity>> GetByRoundFishPrawnCrab(long roundId, string name, byte gameType, DateTime fromDate, DateTime toDate)
        {
            var roundCollection = database.GetCollection<FishPrawnCrabRoundEntity>(Document.GetCollectionName(typeof(FishPrawnCrabRoundEntity)));
            var betCollection = database.GetCollection<FishPrawnCrabBetEntity>(Document.GetCollectionName(typeof(FishPrawnCrabBetEntity)));

            var aggregateBuilder = roundCollection.Aggregate()
                .Match(round => round.Time >= fromDate && round.Time < toDate && round.Number == roundId)
                .Lookup<FishPrawnCrabRoundEntity, FishPrawnCrabBetEntity, FishPrawnCrabRoundWithBetsEntity>(
                    foreignCollection: betCollection,
                    localField: round => round.Number,
                    foreignField: bet => bet.Round,
                    @as: r => r.Bet
                )
                .Unwind<FishPrawnCrabRoundWithBetsEntity, FishPrawnCrabRoundWithBetUnwindEntity>(x => x.Bet)
                .Match(x => x.Bet.Name == name && x.Bet.Paid);

            var unwindEntities = await aggregateBuilder.ToListAsync();
            var results = unwindEntities.Select(x => x.ToGameReport()).ToList();

            return results;
        }

        private async Task<IList<BetHistoryReportEntity>> GetByRoundShakeThePlate(long roundId, string name, byte gameType, DateTime fromDate, DateTime toDate)
        {
            var roundCollection = database.GetCollection<ShakeThePlateRoundEntity>(Document.GetCollectionName(typeof(ShakeThePlateRoundEntity)));
            var betCollection = database.GetCollection<ShakeThePlateBetEntity>(Document.GetCollectionName(typeof(ShakeThePlateBetEntity)));

            var aggregateBuilder = roundCollection.Aggregate()
                .Match(round => round.Time >= fromDate && round.Time < toDate && round.Number == roundId)
                .Lookup<ShakeThePlateRoundEntity, ShakeThePlateBetEntity, ShakeThePlateRoundWithBetsEntity>(
                    foreignCollection: betCollection,
                    localField: round => round.Number,
                    foreignField: bet => bet.Round,
                    @as: r => r.Bet
                )
                .Unwind<ShakeThePlateRoundWithBetsEntity, ShakeThePlateRoundWithBetUnwindEntity>(x => x.Bet)
                .Match(x => x.Bet.Name == name && x.Bet.Paid);

            var unwindEntities = await aggregateBuilder.ToListAsync();
            var results = unwindEntities.Select(x => x.ToGameReport()).ToList();

            return results;
        }
    }
}