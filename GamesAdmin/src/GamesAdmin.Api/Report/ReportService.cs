using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace GamesAdmin.Api.Report
{
    public interface IReportService
    {
        Task<GameReport> GetBigSmallReportAsync(long roundId, string nickname, bool excludeBot);

        Task<IEnumerable<BaseBetHistory>> GetBetHistoryReport(string name, long roundId, byte gameType);

        Task<GameReport> GetBigSmallTurboReportAsync(long roundId, string nickname, bool excludeBot);

        Task<GameReport> GetOddEvenReportAsync(long roundId, string nickname, bool excludeBot);

        Task<GameReport> GetOddEvenTurboReportAsync(long roundId, string nickname, bool excludeBot);
    }

    public class ReportService : IReportService
    {
        private readonly IServiceProvider serviceProvider;

        public ReportService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<GameReport> GetBigSmallReportAsync(long roundId, string nickname, bool excludeBot)
        {
            var repository = serviceProvider.GetService<IBigSmallReportRepository>();
            var result = await repository.GetAsync(roundId, nickname, excludeBot);

            return GenerateGameReport(result);
        }

        public async Task<GameReport> GetBigSmallTurboReportAsync(long roundId, string nickname, bool excludeBot)
        {
            var repository = serviceProvider.GetService<IBigSmallTurboReportRepository>();
            var result = await repository.GetAsync(roundId, nickname, excludeBot);
            return GenerateGameReport(result);
        }

        public async Task<GameReport> GetOddEvenReportAsync(long roundId, string nickname, bool excludeBot)
        {
            var repository = serviceProvider.GetService<IOddEvenReportRepository>();
            var result = await repository.GetAsync(roundId, nickname, excludeBot);

            return GenerateGameReport(result);
        }

        public async Task<GameReport> GetOddEvenTurboReportAsync(long roundId, string nickname, bool excludeBot)
        {
            var repository = serviceProvider.GetService<IOddEvenTurboReportRepository>();
            var result = await repository.GetAsync(roundId, nickname, excludeBot);

            return GenerateGameReport(result);
        }

        public async Task<IEnumerable<BaseBetHistory>> GetBetHistoryReport(string name, long roundId, byte gameType)
        {
            switch (gameType)
            {
                case (byte) GameType.BigSmall:
                    return await GetBigSmallBetHistoryAsync(name, roundId);
                case (byte) GameType.BigSmallTurbo:
                    return await GetBigSmallTurboBetHistoryAsync(name, roundId);
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
            else if (entity is BigSmallBetHistoryReportEntity)
            {
                BigSmallBetHistoryReportEntity bsEntity = (BigSmallBetHistoryReportEntity)entity;

                var betResult = new BigSmallBetResult(bsEntity.FirstDice, bsEntity.SecondDice, bsEntity.ThirdDice);
                var betChoice = new BigSmallChoice(bsEntity.Select, bsEntity.Bet);

                result = new BigSmallBet(entity.Round, entity.Time, entity.BetWin, betResult, betChoice, bsEntity.Refund);
            }

            return result;
        }

        private GameReport GenerateGameReport((BaseRoundEntity, IEnumerable<BetReportEntity>) result)
        {
            return new GameReport
            {
                RoundResult = new RoundResult
                {
                    Number = result.Item1.Number,
                    Dice1 = result.Item1.Dice1,
                    Dice2 = result.Item1.Dice2,
                    Dice3 = result.Item1.Dice3
                },
                Records = result.Item2.Select(x => new GameRecord
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
    }
}