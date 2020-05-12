using System.Collections.Generic;
using GamesAdmin.Core.Enumeration.BetChoices;

namespace GamesAdmin.Core.Enumeration
{
    public static class BetChoiceMapper
    {
        public static IEnumerable<Enumeration> Map(GameType gameType)
        {
            switch (gameType.Value)
            {
                case GameType.BigSmallValue:
                case GameType.BigSmallTurboValue:
                    return Enumeration.GetAll<BigSmallBetChoice>();

                case GameType.OddEvenValue:
                case GameType.OddEvenTurboValue:
                    return Enumeration.GetAll<OddEvenBetChoice>();

                case GameType.ShakeThePlateValue:
                    return Enumeration.GetAll<ShareThePlateBetChoice>();

                case GameType.FishPrawnCrabValue:
                    return Enumeration.GetAll<FishPrawnCrabBetChoice>();

                case GameType.DragonTigerValue:
                    return Enumeration.GetAll<DragonTigerBetChoice>();

                case GameType.KenoProMaxValue:
                    return Enumeration.GetAll<KenoProMaxBetChoice>();

                default: return default;
            }
        }
    }
}
