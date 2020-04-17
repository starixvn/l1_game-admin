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
                GetAllShakeThePlate
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
                GetByRoundShakeThePlate
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
                return new List<GameReportEntity>();
            }

            return new List<GameReportEntity>();
        }

        private async Task<IList<GameReportEntity>> GetAllFishPrawnCrab(string name, byte gameType, DateTime fromDate, DateTime toDate)
        {
            if (gameType == (byte)GameType.FishPrawnCrab)
            {
                var roundCollection = database.GetCollection<FishPrawnCrabRoundEntity>("baucua_phiens");
                var betCollection = database.GetCollection<FishPrawnCrabBetEntity>("baucua_cuocs");

                var aggregateBuilder = roundCollection.Aggregate()
                    .Match(round => round.time >= fromDate && round.time < toDate)
                    .Lookup<FishPrawnCrabRoundEntity, FishPrawnCrabBetEntity, FishPrawnCrabRoundWithBetsEntity>(
                        foreignCollection: betCollection,
                        localField: round => round.Number,
                        foreignField: bet => bet.Round,
                        @as: r => r.Bet
                    )
                    .Unwind<FishPrawnCrabRoundWithBetsEntity, FishPrawnCrabRoundWithBetUnwindEntity>(x => x.Bet)
                    .Match(x => x.Bet.Name == name);

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
                var roundCollection = database.GetCollection<ShakeThePlateRoundEntity>("xocxoc_phiens");
                var betCollection = database.GetCollection<ShakeThePlateBetEntity>("xocxoc_cuocs");

                var aggregateBuilder = roundCollection.Aggregate()
                    .Match(round => round.time >= fromDate && round.time < toDate)
                    .Lookup<ShakeThePlateRoundEntity, ShakeThePlateBetEntity, ShakeThePlateRoundWithBetsEntity>(
                        foreignCollection: betCollection,
                        localField: round => round.Number,
                        foreignField: bet => bet.Round,
                        @as: r => r.Bet
                    )
                    .Unwind<ShakeThePlateRoundWithBetsEntity, ShakeThePlateRoundWithBetUnwindEntity>(x => x.Bet)
                    .Match(x => x.Bet.Name == name);

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
                return new List<GameReportEntity>();
            }

            return new List<GameReportEntity>();
        }

        private async Task<IList<GameReportEntity>> GetByRoundFishPrawnCrab(long roundId, string name, byte gameType, DateTime fromDate, DateTime toDate)
        {
            if (gameType == (byte)GameType.FishPrawnCrab)
            {
                var roundCollection = database.GetCollection<FishPrawnCrabRoundEntity>("baucua_phiens");
                var betCollection = database.GetCollection<FishPrawnCrabBetEntity>("baucua_cuocs");

                var aggregateBuilder = roundCollection.Aggregate()
                    .Match(round => round.time >= fromDate && round.time < toDate && round.Number == roundId)
                    .Lookup<FishPrawnCrabRoundEntity, FishPrawnCrabBetEntity, FishPrawnCrabRoundWithBetsEntity>(
                        foreignCollection: betCollection,
                        localField: round => round.Number,
                        foreignField: bet => bet.Round,
                        @as: r => r.Bet
                    )
                    .Unwind<FishPrawnCrabRoundWithBetsEntity, FishPrawnCrabRoundWithBetUnwindEntity>(x => x.Bet)
                    .Match(x => x.Bet.Name == name);

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
                var roundCollection = database.GetCollection<ShakeThePlateRoundEntity>("xocxoc_phiens");
                var betCollection = database.GetCollection<ShakeThePlateBetEntity>("xocxoc_cuocs");

                var aggregateBuilder = roundCollection.Aggregate()
                    .Match(round => round.time >= fromDate && round.time < toDate && round.Number == roundId)
                    .Lookup<ShakeThePlateRoundEntity, ShakeThePlateBetEntity, ShakeThePlateRoundWithBetsEntity>(
                        foreignCollection: betCollection,
                        localField: round => round.Number,
                        foreignField: bet => bet.Round,
                        @as: r => r.Bet
                    )
                    .Unwind<ShakeThePlateRoundWithBetsEntity, ShakeThePlateRoundWithBetUnwindEntity>(x => x.Bet)
                    .Match(x => x.Bet.Name == name);

                var unwindEntities = await aggregateBuilder.ToListAsync();
                var results = unwindEntities.Select(x => x.ToGameReport()).ToList();

                return results;
            }

            return new List<GameReportEntity>();
        }
    }

    public enum GameType
    {
        BigSmall = 1,
        ShakeThePlate = 2,
        FishPrawnCrab = 3
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
                Time = time,
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
                Time = time,
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
}