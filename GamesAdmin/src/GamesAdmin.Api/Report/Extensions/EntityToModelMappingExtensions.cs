using GamesAdmin.Core.Models;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;

namespace GamesAdmin.Api.Report.Extensions
{
    public static class EntityToModelMappingExtensions
    {
        public static SeDieRoundResult ToModel(this SeDieRoundEntity entity)
        {
            return new SeDieRoundResult
            {
                Number = entity.Number,
                Chips = new[]
                {
                    entity.Result.FirstRedChip ,
                    entity.Result.SecondRedChip,
                    entity.Result.ThirdRedChip ,
                    entity.Result.FourthRedChip
                },
                Even = entity.SettlementResult.Even,
                Odd = entity.SettlementResult.Odd,
                ThreeRed = entity.SettlementResult.ThreeRed,
                FourRed = entity.SettlementResult.FourRed,
                ThreeWhite = entity.SettlementResult.ThreeWhite,
                FourWhite = entity.SettlementResult.FourWhite,
                TwoRed = entity.SettlementResult.TwoRed
            };
        }

        public static SeDieBetRecord ToModel(this SeDieBetReportEntity entity)
        {
            return new SeDieBetRecord
            {
                Time = entity.Time,
                Nickname = entity.Nickname,
                Username = entity.Username,
                BetWin = entity.BetWin,
                Even = entity.Even,
                Odd = entity.Odd,
                FourRed = entity.FourRed,
                FourWhite = entity.FourWhite,
                ThreeRed = entity.ThreeRed,
                ThreeWhite = entity.ThreeWhite,
                TwoRed = entity.TwoRed,
                FreeBet = entity.FreeBet
            };
        }

        public static SeDieRoundResult ToModel(this MiniSeDieRoundEntity entity)
        {
            return new SeDieRoundResult
            {
                Number = entity.Number,
                Chips = new bool[]
                {
                    entity.Result?.FirstRedChip ?? entity.FirstRedChip,
                    entity.Result?.SecondRedChip ?? entity.SecondRedChip,
                    entity.Result?.ThirdRedChip ?? entity.ThirdRedChip,
                    entity.Result?.FourthRedChip ?? entity.FourthRedChip
                },
                Even = entity.SettlementResult.Even,
                Odd = entity.SettlementResult.Odd,
                ThreeRed = entity.SettlementResult.ThreeRed,
                FourRed = entity.SettlementResult.FourRed,
                ThreeWhite = entity.SettlementResult.ThreeWhite,
                FourWhite = entity.SettlementResult.FourWhite
            };
        }
    }
}