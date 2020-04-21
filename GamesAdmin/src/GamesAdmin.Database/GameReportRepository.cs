using GamesAdmin.Database.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Database
{
    public interface IGameReportRepository
    {
        Task<IList<GameReportEntity>> GetAll(string name, byte gameType, DateTime fromDate, DateTime toDate);

        Task<IList<GameReportEntity>> GetByRound(long round, string name, byte gameType, DateTime fromDate, DateTime toDate);
    }

    public class GameReportRepository : IGameReportRepository
    {
        private readonly IMongoDatabase database;

        public GameReportRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<IList<GameReportEntity>> GetAll(string name, byte gameType, DateTime from, DateTime to)
        {
            var tasks = new List<Func<string, byte, DateTime, DateTime, Task<IList<GameReportEntity>>>>
            {
                GetAllBigSmall,
                GetAllFishPrawnCrab,
                GetAllShakeThePlate,
                GetAllBigSmallTurbo
            };

            var result = new List<GameReportEntity>();

            foreach (var task in tasks)
            {
                var taskResult = await task(name, gameType, from, to);
                result.AddRange(taskResult);
            }

            return result;
        }

        public async Task<IList<GameReportEntity>> GetByRound(long round, string name, byte gameType, DateTime from, DateTime to)
        {
            var tasks = new List<Func<long, string, byte, DateTime, DateTime, Task<IList<GameReportEntity>>>>
            {
                GetByRoundBigSmall,
                GetByRoundFishPrawnCrab,
                GetByRoundShakeThePlate,
                GetByRoundBigSmallTurbo
            };

            var result = new List<GameReportEntity>();

            foreach (var task in tasks)
            {
                var taskResult = await task(round, name, gameType, from, to);
                result.AddRange(taskResult);
            }

            return result;
        }

        private async Task<IList<GameReportEntity>> GetAllBigSmall(string name, byte gameType, DateTime fromDate, DateTime toDate)
        {
            if (gameType == (byte)GameType.BigSmall)
            {
                var roundCollection = database.GetCollection<BigSmallRoundEntity>(Document.GetCollectionName(typeof(BigSmallRoundEntity)));
                var oneCollection = database.GetCollection<BigSmallOnesEntity>(Document.GetCollectionName(typeof(BigSmallOnesEntity)));
                var accountInfoCollection = database.GetCollection<AccountInfoEntity>(Document.GetCollectionName(typeof(AccountInfoEntity)));

                var aggregateBuilder = roundCollection.Aggregate()
                    .Match(round => round.Time >= fromDate && round.Time < toDate)
                    .Lookup<BigSmallRoundEntity, BigSmallOnesEntity, BigSmallRoundWithBetsEntity>(
                        foreignCollection: oneCollection,
                        localField: round => round.Number,
                        foreignField: bet => bet.Round,
                        @as: r => r.One
                    )
                    .Unwind<BigSmallRoundWithBetsEntity, BigSmallRoundWithBetUnwindEntity>(x => x.One)
                    .Lookup<BigSmallRoundWithBetUnwindEntity, AccountInfoEntity, BigSmallRoundWithBetUnwindEntity>(
                        foreignCollection: accountInfoCollection,
                        localField: round => round.One.Uid,
                        foreignField: account => account.UserId,
                        @as: r => r.Accounts
                    )
                    .Match<BigSmallRoundWithBetUnwindEntity>(round => round.Accounts.ElementAt(0).Name == name && round.One.Paid);

                var unwindEntities = await aggregateBuilder.ToListAsync();
                var results = unwindEntities.Select(x => x.ToGameReport()).ToList();

                return results;
            }

            return new List<GameReportEntity>();
        }

        private async Task<IList<GameReportEntity>> GetAllFishPrawnCrab(string name, byte gameType, DateTime fromDate, DateTime toDate)
        {
            if (gameType == (byte)GameType.FishPrawnCrab)
            {
                var roundCollection = database.GetCollection<FishPrawnCrabRoundEntity>(Document.GetCollectionName(typeof(FishPrawnCrabRoundEntity)));
                var betCollection = database.GetCollection<FishPrawnCrabBetEntity>(Document.GetCollectionName(typeof(FishPrawnCrabBetEntity)));

                var aggregateBuilder = roundCollection.Aggregate()
                    .Match(round => round.Time >= fromDate && round.Time < toDate)
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

            return new List<GameReportEntity>();
        }

        private async Task<IList<GameReportEntity>> GetAllShakeThePlate(string name, byte gameType, DateTime fromDate, DateTime toDate)
        {
            if (gameType == (byte)GameType.ShakeThePlate)
            {
                var roundCollection = database.GetCollection<ShakeThePlateRoundEntity>(Document.GetCollectionName(typeof(ShakeThePlateRoundEntity)));
                var betCollection = database.GetCollection<ShakeThePlateBetEntity>(Document.GetCollectionName(typeof(ShakeThePlateBetEntity)));

                var aggregateBuilder = roundCollection.Aggregate()
                    .Match(round => round.Time >= fromDate && round.Time < toDate)
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

            return new List<GameReportEntity>();
        }

        private async Task<IList<GameReportEntity>> GetAllBigSmallTurbo(string name, byte gameType, DateTime fromDate, DateTime toDate)
        {
            if (gameType == (byte)GameType.BigSmallTurbo)
            {
                var roundCollection = database.GetCollection<BigSmallTurboRoundEntity>(Document.GetCollectionName(typeof(BigSmallTurboRoundEntity)));
                var oneCollection = database.GetCollection<BigSmallTurboOnesEntity>(Document.GetCollectionName(typeof(BigSmallTurboOnesEntity)));
                var accountInfoCollection = database.GetCollection<AccountInfoEntity>(Document.GetCollectionName(typeof(AccountInfoEntity)));

                var aggregateBuilder = roundCollection.Aggregate()
                    .Match(round => round.Time >= fromDate && round.Time < toDate)
                    .Lookup<BigSmallTurboRoundEntity, BigSmallTurboOnesEntity, BigSmallTurboRoundWithBetsEntity>(
                        foreignCollection: oneCollection,
                        localField: round => round.Number,
                        foreignField: bet => bet.Round,
                        @as: r => r.One
                    )
                    .Unwind<BigSmallTurboRoundWithBetsEntity, BigSmallTurboRoundWithBetUnwindEntity>(x => x.One)
                    .Lookup<BigSmallTurboRoundWithBetUnwindEntity, AccountInfoEntity, BigSmallTurboRoundWithBetUnwindEntity>(
                        foreignCollection: accountInfoCollection,
                        localField: round => round.One.Uid,
                        foreignField: account => account.UserId,
                        @as: r => r.Accounts
                    )
                    .Match<BigSmallTurboRoundWithBetUnwindEntity>(round => round.Accounts.ElementAt(0).Name == name && round.One.Paid);

                var unwindEntities = await aggregateBuilder.ToListAsync();
                var results = unwindEntities.Select(x => x.ToGameReport()).ToList();

                return results;
            }

            return new List<GameReportEntity>();
        }

        private async Task<IList<GameReportEntity>> GetByRoundBigSmall(long roundId, string name, byte gameType, DateTime fromDate, DateTime toDate)
        {
            if (gameType == (byte)GameType.BigSmall)
            {
                var roundCollection = database.GetCollection<BigSmallRoundEntity>(Document.GetCollectionName(typeof(BigSmallRoundEntity)));
                var oneCollection = database.GetCollection<BigSmallOnesEntity>(Document.GetCollectionName(typeof(BigSmallOnesEntity)));
                var accountInfoCollection = database.GetCollection<AccountInfoEntity>(Document.GetCollectionName(typeof(AccountInfoEntity)));

                var aggregateBuilder = roundCollection.Aggregate()
                    .Match(round => round.Time >= fromDate && round.Time < toDate && round.Number == roundId)
                    .Lookup<BigSmallRoundEntity, BigSmallOnesEntity, BigSmallRoundWithBetsEntity>(
                        foreignCollection: oneCollection,
                        localField: round => round.Number,
                        foreignField: bet => bet.Round,
                        @as: r => r.One
                    )
                    .Unwind<BigSmallRoundWithBetsEntity, BigSmallRoundWithBetUnwindEntity>(x => x.One)
                    .Lookup<BigSmallRoundWithBetUnwindEntity, AccountInfoEntity, BigSmallRoundWithBetUnwindEntity>(
                        foreignCollection: accountInfoCollection,
                        localField: round => round.One.Uid,
                        foreignField: account => account.UserId,
                        @as: r => r.Accounts
                    )
                    .Match<BigSmallRoundWithBetUnwindEntity>(round => round.Accounts.ElementAt(0).Name == name && round.One.Paid);

                var unwindEntities = await aggregateBuilder.ToListAsync();
                var results = unwindEntities.Select(x => x.ToGameReport()).ToList();

                return results;
            }

            return new List<GameReportEntity>();
        }

        private async Task<IList<GameReportEntity>> GetByRoundFishPrawnCrab(long roundId, string name, byte gameType, DateTime fromDate, DateTime toDate)
        {
            if (gameType == (byte)GameType.FishPrawnCrab)
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

            return new List<GameReportEntity>();
        }

        private async Task<IList<GameReportEntity>> GetByRoundShakeThePlate(long roundId, string name, byte gameType, DateTime fromDate, DateTime toDate)
        {
            if (gameType == (byte)GameType.ShakeThePlate)
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

            return new List<GameReportEntity>();
        }

        private async Task<IList<GameReportEntity>> GetByRoundBigSmallTurbo(long roundId, string name, byte gameType, DateTime fromDate, DateTime toDate)
        {
            if (gameType == (byte)GameType.BigSmallTurbo)
            {
                var roundCollection = database.GetCollection<BigSmallTurboRoundEntity>(Document.GetCollectionName(typeof(BigSmallTurboRoundEntity)));
                var oneCollection = database.GetCollection<BigSmallTurboOnesEntity>(Document.GetCollectionName(typeof(BigSmallTurboOnesEntity)));
                var accountInfoCollection = database.GetCollection<AccountInfoEntity>(Document.GetCollectionName(typeof(AccountInfoEntity)));

                var aggregateBuilder = roundCollection.Aggregate()
                    .Match(round => round.Time >= fromDate && round.Time < toDate && round.Number == roundId)
                    .Lookup<BigSmallTurboRoundEntity, BigSmallTurboOnesEntity, BigSmallTurboRoundWithBetsEntity>(
                        foreignCollection: oneCollection,
                        localField: round => round.Number,
                        foreignField: bet => bet.Round,
                        @as: r => r.One
                    )
                    .Unwind<BigSmallTurboRoundWithBetsEntity, BigSmallTurboRoundWithBetUnwindEntity>(x => x.One)
                    .Lookup<BigSmallTurboRoundWithBetUnwindEntity, AccountInfoEntity, BigSmallTurboRoundWithBetUnwindEntity>(
                        foreignCollection: accountInfoCollection,
                        localField: round => round.One.Uid,
                        foreignField: account => account.UserId,
                        @as: r => r.Accounts
                    )
                    .Match<BigSmallTurboRoundWithBetUnwindEntity>(round => round.Accounts.ElementAt(0).Name == name && round.One.Paid);

                var unwindEntities = await aggregateBuilder.ToListAsync();
                var results = unwindEntities.Select(x => x.ToGameReport()).ToList();

                return results;
            }

            return new List<GameReportEntity>();
        }
    }

    internal enum GameType
    {
        BigSmall = 1,
        ShakeThePlate = 2,
        FishPrawnCrab = 3,
        BigSmallTurbo = 4,
        EvenOdd = 5,
        EvenOddTurbo = 6
    }

    public abstract class GameReportEntity
    {
        public DateTime Time { get; set; }

        public string Name { get; set; }

        public long Round { get; set; }

        public decimal BetWin { get; set; }
    }

    public class BigSmallGameReportEntity : GameReportEntity
    {
        public byte FirstDice { get; set; }

        public byte SecondDice { get; set; }

        public byte ThirdDice { get; set; }

        public decimal Bet { get; set; }

        public bool SelectBig { get; set; }

        public decimal Refund { get; set; }
    }

    public class FishPrawnCrabReportEntity : GameReportEntity
    {
        public byte FirstDice { get; set; }

        public byte SecondDice { get; set; }

        public byte ThirdDice { get; set; }

        public decimal Stag { get; set; }

        public decimal Gourd { get; set; }

        public decimal Rooster { get; set; }

        public decimal Fish { get; set; }

        public decimal Crab { get; set; }

        public decimal Prawn { get; set; }
    }

    public class ShakeThePlateReportEntity : GameReportEntity
    {
        public bool FirstRedChip { get; set; }

        public bool SecondRedChip { get; set; }

        public bool ThirdRedChip { get; set; }

        public bool FourthRedChip { get; set; }

        public decimal Even { get; set; }

        public decimal Odd { get; set; }

        public decimal ThreeRed { get; set; }

        public decimal FourRed { get; set; }

        public decimal ThreeWhite { get; set; }

        public decimal FourWhite { get; set; }
    }

    internal class FishPrawnCrabRoundWithBetsEntity : FishPrawnCrabRoundEntity
    {
        public IEnumerable<FishPrawnCrabBetEntity> Bet { get; set; }
    }

    internal class FishPrawnCrabRoundWithBetUnwindEntity : FishPrawnCrabRoundEntity
    {
        public FishPrawnCrabBetEntity Bet { get; set; }

        public GameReportEntity ToGameReport()
        {
            return new FishPrawnCrabReportEntity
            {
                Name = Bet.Name,
                BetWin = Bet.Betwin,
                Round = Number,
                Time = Time,
                FirstDice = (byte)(FirstDice + 1),
                SecondDice = (byte)(SecondDice + 1),
                ThirdDice = (byte)(ThirdDice + 1),
                Stag = Bet.Stag,
                Gourd = Bet.Gourd,
                Crab = Bet.Crab,
                Fish = Bet.Fish,
                Prawn = Bet.Prawn,
                Rooster = Bet.Rooster
            };
        }
    }

    internal class ShakeThePlateRoundWithBetsEntity : ShakeThePlateRoundEntity
    {
        public IEnumerable<ShakeThePlateBetEntity> Bet { get; set; }
    }

    internal class ShakeThePlateRoundWithBetUnwindEntity : ShakeThePlateRoundEntity
    {
        public ShakeThePlateBetEntity Bet { get; set; }

        public GameReportEntity ToGameReport()
        {
            return new ShakeThePlateReportEntity
            {
                Name = Bet.Name,
                BetWin = Bet.Betwin,
                Round = Number,
                Time = Time,
                Even = Bet.Even,
                Odd = Bet.Odd,
                ThreeRed = Bet.ThreeRed,
                FourRed = Bet.FourRed,
                ThreeWhite = Bet.ThreeWhite,
                FourWhite = Bet.FourWhite,
                FirstRedChip = FirstRedChip,
                SecondRedChip = SecondRedChip,
                ThirdRedChip = ThirdRedChip,
                FourthRedChip = FourthRedChip
            };
        }
    }

    internal class BigSmallRoundWithBetsEntity : BigSmallRoundEntity
    {
        public IEnumerable<BigSmallOnesEntity> One { get; set; }

        public IEnumerable<AccountInfoEntity> Accounts { get; set; }
    }

    internal class BigSmallRoundWithBetUnwindEntity : BigSmallRoundEntity
    {
        public BigSmallOnesEntity One { get; set; }

        public IEnumerable<AccountInfoEntity> Accounts { get; set; }

        public GameReportEntity ToGameReport()
        {
            return new BigSmallGameReportEntity
            {
                BetWin = One.Betwin,
                Round = Number,
                Time = Time,
                Bet = One.Bet,
                FirstDice = (byte)Dice1,
                SecondDice = (byte)Dice2,
                ThirdDice = (byte)Dice3,
                Refund = One.Back,
                SelectBig = One.Select
            };
        }
    }

    internal class BigSmallTurboRoundWithBetsEntity : BigSmallTurboRoundEntity
    {
        public IEnumerable<BigSmallTurboOnesEntity> One { get; set; }

        public IEnumerable<AccountInfoEntity> Accounts { get; set; }
    }

    internal class BigSmallTurboRoundWithBetUnwindEntity : BigSmallTurboRoundEntity
    {
        public BigSmallTurboOnesEntity One { get; set; }

        public IEnumerable<AccountInfoEntity> Accounts { get; set; }

        public GameReportEntity ToGameReport()
        {
            return new BigSmallGameReportEntity
            {
                BetWin = One.Betwin,
                Round = Number,
                Time = Time,
                Bet = One.Bet,
                FirstDice = (byte)Dice1,
                SecondDice = (byte)Dice2,
                ThirdDice = (byte)Dice3,
                Refund = One.Back,
                SelectBig = One.Select
            };
        }
    }

    internal class EvenOddRoundWithBetsEntity : EvenOddRoundEntity
    {
        public IEnumerable<EvenOddOnesEntity> One { get; set; }

        public IEnumerable<AccountInfoEntity> Accounts { get; set; }
    }

    internal class EvenOddRoundWithBetUnwindEntity : EvenOddRoundEntity
    {
        public EvenOddOnesEntity One { get; set; }

        public IEnumerable<AccountInfoEntity> Accounts { get; set; }

        public GameReportEntity ToGameReport()
        {
            return new BigSmallGameReportEntity
            {
                BetWin = One.Betwin,
                Round = Number,
                Time = Time,
                Bet = One.Bet,
                FirstDice = (byte)Dice1,
                SecondDice = (byte)Dice2,
                ThirdDice = (byte)Dice3,
                Refund = One.Back,
                SelectBig = One.Select
            };
        }
    }

}