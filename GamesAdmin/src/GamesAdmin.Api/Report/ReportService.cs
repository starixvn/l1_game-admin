using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Report
{
    public interface IReportService
    {
        Task<GameReport> GetBigSmallReportAsync(long roundId, string nickname, bool excludeBot);

        Task<IList<Bet>> GetGameReport(string name, long roundId, byte gameType, DateTime from, DateTime to);

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

        public async Task<IList<Bet>> GetGameReport(string name, long roundId, byte gameType, DateTime from, DateTime to)
        {
            var gameReportRepository = serviceProvider.GetService<IGameReportRepository>();

            IList<GameReportEntity> gameReportEntities;

            if (roundId == 0)
            {
               gameReportEntities = await gameReportRepository.GetAll(name, gameType, from, to);
                
            } else
            {
                gameReportEntities = await gameReportRepository.GetByRound(roundId, name, gameType, from, to);
            }

            var results = gameReportEntities.Select(TransformGameReportEntityToBetModel).ToList();

            return results;
        }

        private Bet TransformGameReportEntityToBetModel(GameReportEntity entity)
        {
            Bet result = null;

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
            else if (entity is BigSmallGameReportEntity)
            {
                BigSmallGameReportEntity bsEntity = (BigSmallGameReportEntity)entity;

                var betResult = new BigSmallBetResult(bsEntity.FirstDice, bsEntity.SecondDice, bsEntity.ThirdDice);
                var betChoice = new BigSmallChoice(bsEntity.SelectBig, bsEntity.Bet);

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