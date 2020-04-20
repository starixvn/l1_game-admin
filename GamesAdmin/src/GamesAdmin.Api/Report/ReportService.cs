using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Report
{
    public interface IReportService
    {
        Task<BigSmallReport> GetBigSmallReportAsync(long roundId, string nickname, bool excludeBot);

        Task<IList<Bet>> GetGameReport(string name, long roundId, byte gameType, DateTime from, DateTime to);
    }

    public class ReportService : IReportService
    {
        private readonly IBigSmallReportRepository bigSmallReportRepository;
        private readonly IGameReportRepository gameReportRepository;

        public ReportService(
            IBigSmallReportRepository bigSmallReportRepository,
            IGameReportRepository gameReportRepository)
        {
            this.bigSmallReportRepository = bigSmallReportRepository;
            this.gameReportRepository = gameReportRepository;
        }

        public async Task<BigSmallReport> GetBigSmallReportAsync(long roundId, string nickname, bool excludeBot)
        {
            var result = await bigSmallReportRepository.GetAsync(roundId, nickname, excludeBot);

            return new BigSmallReport
            {
                RoundResult = new RoundResult
                {
                    Number = result.Item1.Number,
                    Dice1 = result.Item1.Dice1,
                    Dice2 = result.Item1.Dice2,
                    Dice3 = result.Item1.Dice3
                },
                Records = result.Item2.Select(x => new BigSmallRecord
                {
                    Time = x.Time,
                    Nickname = x.Nickname,
                    Username = x.Username,
                    SelectBig = x.SelectBig,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    Back = x.Back
                })
            };
        }

        public async Task<IList<Bet>> GetGameReport(string name, long roundId, byte gameType, DateTime from, DateTime to)
        {
            IList<GameReportEntity> gameReportEntities;

            if (roundId == 0)
            {
                gameReportEntities = await this.gameReportRepository.GetAll(name, gameType, from, to);
            }
            else
            {
                gameReportEntities = await this.gameReportRepository.GetByRound(roundId, name, gameType, from, to);
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
    }
}