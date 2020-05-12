using System.Collections.Generic;
using GamesAdmin.Core.Enumeration.BetChoices;

namespace GamesAdmin.Core.Enumeration
{
    public static class BetChoiceMapper
    {
        public static IEnumerable<Enumeration> Map(GameType game)
        {
            switch (game)
            {
                case GameType.BigSmall:
                case GameType.BigSmallTurbo:
                    return Enumeration.GetAll<BigSmallBetChoice>();

                case GameType.EvenOdd:
                case GameType.EvenOddTurbo:
                    return Enumeration.GetAll<OddEvenBetChoice>();

                case GameType.ShakeThePlate:
                    return Enumeration.GetAll<ShareThePlateBetChoice>();

                case GameType.FishPrawnCrab:
                    return Enumeration.GetAll<FishPrawnCrabBetChoice>();

                case GameType.DragonTiger:
                    return Enumeration.GetAll<DragonTigerBetChoice>();

                case GameType.KenoProMax:
                    return Enumeration.GetAll<KenoProMaxBetChoice>();

                default: return default;
            }
        }
    }
}
