using System.Collections.Generic;
using GamesAdmin.Core.Enumeration.BetChoices;
using GamesAdmin.Core.Enumeration.BetOdds;

namespace GamesAdmin.Core.Enumeration
{
    public static class BetOddsMapper
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
                case GameType.SpecialSeDieValue:
                    return Enumeration.GetAll<OddEvenBetChoice>();

                case GameType.SeDieValue:
                    return Enumeration.GetAll<SeDieBetChoice>();

                case GameType.FishPrawnCrabValue:
                    return Enumeration.GetAll<FishPrawnCrabBetChoice>();

                case GameType.DragonTigerValue:
                    return Enumeration.GetAll<DragonTigerBetChoice>();

                case GameType.KenoProMaxValue:
                case GameType.KenoProVietValue:
                case GameType.KenoMaxValue:
                case GameType.KenoMax2Value:
                case GameType.KenoMiniValue:
                case GameType.KenoMini2Value:
                case GameType.KenoEastValue:
                case GameType.KenoWestValue:
                case GameType.KenoSouthValue:
                case GameType.KenoNorthValue:
                    return Enumeration.GetAll<KenoBetChoice>();

                case GameType.MiniSeDieValue:
                    return Enumeration.GetAll<MiniSeDieBetChoice>();

                case GameType.BaccaratValue:
                    return Enumeration.GetAll<BaccaratBetChoice>();

                case GameType.RouletteValue:
                    return Enumeration.GetAll<RouletteBetChoice>();

                case GameType.VipSeDieValue:
                    return Enumeration.GetAll<VipSeDieBetChoice>();

                case GameType.VipFishPrawnCrabValue:
                    return Enumeration.GetAll<VipFishPrawnCrabBetOdds>();

                case GameType.BlackJackValue:
                    return Enumeration.GetAll<BlackjackBetChoice>();
                    
                case GameType.VipSicBoValue:
                    return Enumeration.GetAll<VipSicBoBetChoice>();

                default: return default;
            }
        }
    }
}
