using GamesAdmin.Core.Models;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;

namespace GamesAdmin.Api.Report.Extensions
{
    public static class EntityToModelMappingExtensions
    {
        public static SeDieRoundResult ToModel(this ShakeThePlateRoundEntity entity)
        {
            return new SeDieRoundResult
            {
                Number = entity.Number,
                Chips = new bool[]
                {
                    entity.FirstRedChip,
                    entity.SecondRedChip,
                    entity.ThirdRedChip,
                    entity.FourthRedChip
                }
            };
        }

        public static SeDieBetRecord ToModel(this ShakeThePlateBetReportEntity entity)
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
                ThreeWhite = entity.ThreeWhite
            };
        }

        public static SeDieRoundResult ToModel(this MiniSeDieRoundEntity entity)
        {
            return new SeDieRoundResult
            {
                Number = entity.Number,
                Chips = new bool[]
                {
                    entity.FirstRedChip,
                    entity.SecondRedChip,
                    entity.ThirdRedChip,
                    entity.FourthRedChip
                }
            };
        }

    }
}