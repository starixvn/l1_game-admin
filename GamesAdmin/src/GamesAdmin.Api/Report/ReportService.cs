using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Api.Report.Extensions;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Enumeration.BetChoices;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.RoundResults;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Entities.Keno;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace GamesAdmin.Api.Report
{
    public interface IReportService
    {
        Task<BigSmallBetReport> GetBigSmallReportAsync(long roundId, string nickname, bool excludeBot);

        Task<IEnumerable<BaseBetHistory>> GetBetHistoryReport(string name, long roundId, byte gameType);

        Task<BigSmallBetReport> GetBigSmallTurboReportAsync(long roundId, string nickname, bool excludeBot);

        Task<OddEvenBetReport> GetOddEvenReportAsync(long roundId, string nickname, bool excludeBot);

        Task<OddEvenBetReport> GetOddEvenTurboReportAsync(long roundId, string nickname, bool excludeBot);

        Task<SeDieBetReport> GetSeDieReportAsync(long roundId, string nickname, bool excludeBot);

        Task<SeDieBetReport> GetMiniSeDieReportAsync(long roundId, string nickname, bool excludeBot);
        
        Task<KenoBetReport> GetKenoProVietReportAsync(long roundId, string nickname);
    }

    public class ReportService : IReportService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IGenericRepository<KenoProVietOnesEntity> kenoProVietOnesRepository;
        private readonly IGenericRepository<KenoRoundEntity> kenoProvietRoundRepository;
        private readonly IGenericRepository<AccountEntity> accountRepository;

        public ReportService(
            IServiceProvider serviceProvider,
            IGenericRepository<KenoProVietOnesEntity> kenoProVietOnesRepository,
            IGenericRepository<KenoRoundEntity> kenoProvietRoundRepository,
            IGenericRepository<AccountEntity> accountRepository)
        {
            this.serviceProvider = serviceProvider;
            this.kenoProVietOnesRepository = kenoProVietOnesRepository;
            this.kenoProvietRoundRepository = kenoProvietRoundRepository;
            this.accountRepository = accountRepository;
        }

        public async Task<BigSmallBetReport> GetBigSmallReportAsync(long roundId, string nickname, bool excludeBot)
        {
            var repository = serviceProvider.GetService<IBigSmallReportRepository>();
            var result = await repository.GetAsync(roundId, nickname, excludeBot);

            return new BigSmallBetReport
            {
                RoundResult = new BigSmallRoundResult
                {
                    Number = result.Item1.Number,
                    Dice1 = result.Item1.Dice1,
                    Dice2 = result.Item1.Dice2,
                    Dice3 = result.Item1.Dice3
                },
                Records = result.Item2.Select(x => new BigSmallBetRecord
                {
                    Time = x.Time,
                    Nickname = x.Nickname,
                    Username = x.Username,
                    Select = x.Select,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    Back = x.Back
                })
            };
        }

        public async Task<BigSmallBetReport> GetBigSmallTurboReportAsync(long roundId, string nickname, bool excludeBot)
        {
            var repository = serviceProvider.GetService<IBigSmallTurboReportRepository>();
            var result = await repository.GetAsync(roundId, nickname, excludeBot);

            return new BigSmallBetReport
            {
                RoundResult = new BigSmallRoundResult
                {
                    Number = result.Item1.Number,
                    Dice1 = result.Item1.Dice1,
                    Dice2 = result.Item1.Dice2,
                    Dice3 = result.Item1.Dice3
                },
                Records = result.Item2.Select(x => new BigSmallBetRecord
                {
                    Time = x.Time,
                    Nickname = x.Nickname,
                    Username = x.Username,
                    Select = x.Select,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    Back = x.Back
                })
            };
        }

        public async Task<OddEvenBetReport> GetOddEvenReportAsync(long roundId, string nickname, bool excludeBot)
        {
            var repository = serviceProvider.GetService<IOddEvenReportRepository>();
            var result = await repository.GetAsync(roundId, nickname, excludeBot);

            return new OddEvenBetReport
            {
                RoundResult = new OddEvenRoundResult
                {
                    Number = result.Item1.Number,
                    Dice1 = result.Item1.Dice1,
                    Dice2 = result.Item1.Dice2,
                    Dice3 = result.Item1.Dice3
                },
                Records = result.Item2.Select(x => new OddEvenBetRecord
                {
                    Time = x.Time,
                    Nickname = x.Nickname,
                    Username = x.Username,
                    Select = x.Select,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    Back = x.Back
                })
            };
        }

        public async Task<OddEvenBetReport> GetOddEvenTurboReportAsync(long roundId, string nickname, bool excludeBot)
        {
            var repository = serviceProvider.GetService<IOddEvenTurboReportRepository>();
            var result = await repository.GetAsync(roundId, nickname, excludeBot);

            return new OddEvenBetReport
            {
                RoundResult = new OddEvenRoundResult
                {
                    Number = result.Item1.Number,
                    Dice1 = result.Item1.Dice1,
                    Dice2 = result.Item1.Dice2,
                    Dice3 = result.Item1.Dice3
                },
                Records = result.Item2.Select(x => new OddEvenBetRecord
                {
                    Time = x.Time,
                    Nickname = x.Nickname,
                    Username = x.Username,
                    Select = x.Select,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    Back = x.Back
                })
            };
        }

        public async Task<SeDieBetReport> GetSeDieReportAsync(long roundId, string nickname, bool excludeBot)
        {
            var repository = serviceProvider.GetService<IShakeThePlateReportRepository>();
            var result = await repository.GetAsync(roundId, nickname, excludeBot);

            return new SeDieBetReport
            {
                RoundResult = result.Item1.ToModel(),
                Records = result.Item2.Select(x => x.ToModel())
            };
        }

        public async Task<SeDieBetReport> GetMiniSeDieReportAsync(long roundId, string nickname, bool excludeBot)
        {
            var repository = serviceProvider.GetService<IMiniSeDieReportRepository>();
            var result = await repository.GetAsync(roundId, nickname, excludeBot);

            return new SeDieBetReport
            {
                RoundResult = result.Item1.ToModel(),
                Records = result.Item2.Select(x => x.ToModel())
            };
        }

        public async Task<IEnumerable<BaseBetHistory>> GetBetHistoryReport(string name, long roundId, byte gameType)
        {
            switch (gameType)
            {
                case (byte)GameId.BigSmall:
                    return await GetBigSmallBetHistoryAsync(name, roundId);

                case (byte)GameId.BigSmallTurbo:
                    return await GetBigSmallTurboBetHistoryAsync(name, roundId);

                case (byte)GameId.EvenOdd:
                    return await GetOddEvenBetHistoryAsync(name, roundId);

                case (byte)GameId.EvenOddTurbo:
                    return await GetOddEvenTurboBetHistoryAsync(name, roundId);

                default:
                    break;
            }

            return await GetBigSmallBetHistoryAsync(name, roundId);
        }

        private async Task<IEnumerable<BigSmallBet>> GetBigSmallBetHistoryAsync(string name, long roundId)
        {
            var betHistoryReportRepository = serviceProvider.GetService<IBetHistoryReportRepository>();
            var result = await betHistoryReportRepository.GetBigSmallAsync(roundId, name);

            return result.Select(x =>
            {
                var betResult = new BigSmallBetResult(x.FirstDice, x.SecondDice, x.ThirdDice);
                var betChoice = new BigSmallChoice(x.Select, x.Bet);

                return new BigSmallBet(x.Round, x.Time, x.BetWin, betResult, betChoice, x.Refund);
            });
        }

        private async Task<IEnumerable<BigSmallBet>> GetBigSmallTurboBetHistoryAsync(string name, long roundId)
        {
            var betHistoryReportRepository = serviceProvider.GetService<IBetHistoryReportRepository>();
            var result = await betHistoryReportRepository.GetBigSmallTurboAsync(roundId, name);

            return result.Select(x =>
            {
                var betResult = new BigSmallBetResult(x.FirstDice, x.SecondDice, x.ThirdDice);
                var betChoice = new BigSmallChoice(x.Select, x.Bet);

                return new BigSmallBet(x.Round, x.Time, x.BetWin, betResult, betChoice, x.Refund);
            });
        }

        private async Task<IEnumerable<OddEvenBet>> GetOddEvenBetHistoryAsync(string name, long roundId)
        {
            var betHistoryReportRepository = serviceProvider.GetService<IBetHistoryReportRepository>();
            var result = await betHistoryReportRepository.GetOddEvenAsync(roundId, name);

            return result.Select(x =>
            {
                var betResult = new OddEvenBetResult(x.FirstDice, x.SecondDice, x.ThirdDice);
                var betChoice = new OddEvenChoice(x.Select, x.Bet);

                return new OddEvenBet(x.Round, x.Time, x.BetWin, betResult, betChoice, x.Refund);
            });
        }

        private async Task<IEnumerable<OddEvenBet>> GetOddEvenTurboBetHistoryAsync(string name, long roundId)
        {
            var betHistoryReportRepository = serviceProvider.GetService<IBetHistoryReportRepository>();
            var result = await betHistoryReportRepository.GetOddEvenTurboAsync(roundId, name);

            return result.Select(x =>
            {
                var betResult = new OddEvenBetResult(x.FirstDice, x.SecondDice, x.ThirdDice);
                var betChoice = new OddEvenChoice(x.Select, x.Bet);

                return new OddEvenBet(x.Round, x.Time, x.BetWin, betResult, betChoice, x.Refund);
            });
        }

        private BaseBetHistory TransformGameReportEntityToBetModel(BetHistoryReportEntity entity)
        {
            BaseBetHistory result = null;

            if (entity is FishPrawnCrabReportEntity)
            {
                FishPrawnCrabReportEntity fpcEntity = (FishPrawnCrabReportEntity)entity;

                var betResult = new FishPrawnCrabBetResult(fpcEntity.FirstDice, fpcEntity.SecondDice, fpcEntity.ThirdDice);
                var betChoice = new FishPrawnCrabChoice(fpcEntity.Stag, fpcEntity.Gourd, fpcEntity.Rooster, fpcEntity.Fish, fpcEntity.Crab, fpcEntity.Prawn);

                result = new FishPrawnCrabBet(entity.Round, entity.Time, entity.BetWin, betResult, betChoice);
            }
            else if (entity is ShakeThePlateReportEntity)
            {
                ShakeThePlateReportEntity stpEntity = (ShakeThePlateReportEntity)entity;

                var betResult = new ShakeThePlateBetResult(stpEntity.FirstRedChip, stpEntity.SecondRedChip, stpEntity.ThirdRedChip, stpEntity.FourthRedChip);
                var betChoice = new ShakeThePlateChoice(stpEntity.Even, stpEntity.Odd, stpEntity.ThreeRed, stpEntity.FourRed, stpEntity.ThreeWhite, stpEntity.FourWhite);

                result = new ShakeThePlateBet(entity.Round, entity.Time, entity.BetWin, betResult, betChoice);
            }

            return result;
        }

        public async Task<KenoBetReport> GetKenoProVietReportAsync(long roundId, string nickname)
        {
            var round = await kenoProvietRoundRepository.FindOneAsync(round => round.Number == roundId);

            var roundResult = round == null
                ? default
                : new KenoRoundResult(round.Result.ToArray(), new KenoSettlementResult(round.SettlementResult));

            var newBets = await kenoProVietOnesRepository
                   .Aggregate()
                   .Match(x => x.Round == roundId)
                   .AppendStage<KenoProVietOnesEntity>("{$addFields: {userId: {$toObjectId: '$uid'}}}")
                   .Lookup<KenoProVietOnesEntity, AccountEntity, LookedUpKenoOnes>(
                       accountRepository.Collection(),
                       x => x.UserId,
                       y => y.Id,
                       y => y.InnerUsers
                   )
                   .ToListAsync();

            var betRecords = newBets
                .Select(bet => new KenoBetRecord(ConvertToAmountByChoice(bet), bet.Odds)
                {
                    Nickname = bet.Name,
                    BetWin = bet.Betwin,
                    Time = bet.Time,
                    Username = bet.InnerUsers.FirstOrDefault().Local.Username
                })
                .ToList();

            return new KenoBetReport(roundResult, betRecords);
        }

        private static Dictionary<string, decimal> ConvertToAmountByChoice(KenoProVietOnesEntity entity)
        {
            var amounts = new Dictionary<string, decimal>();

            amounts[KenoBetChoice.BigValue] = entity.Big;
            amounts[KenoBetChoice.SmallValue] = entity.Small;
            amounts[KenoBetChoice.OddValue] = entity.Odd;
            amounts[KenoBetChoice.EvenValue] = entity.Even;
            amounts[KenoBetChoice.GoldValue] = entity.Gold;
            amounts[KenoBetChoice.EarthValue] = entity.Earth;
            amounts[KenoBetChoice.FireValue] = entity.Fire;
            amounts[KenoBetChoice.WaterValue] = entity.Water;
            amounts[KenoBetChoice.WoodValue] = entity.Wood;

            return amounts;
        }
    }

    public class LookedUpKenoOnes : KenoProVietOnesEntity
    {
        public IEnumerable<AccountEntity> InnerUsers { get; set; }
    }
}