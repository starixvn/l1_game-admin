using System.Collections.Generic;
using GamesAdmin.Core.Enumeration.BetChoices;
using GamesAdmin.Core.Enumeration.BetOdds;

namespace GamesAdmin.Core.Enumeration
{
    public static class BetOddsMapper
    {
        public static IEnumerable<Enumeration> Map(Game gameType)
        {
            switch (gameType.Value)
            {
                case Game.BigSmallValue:
                case Game.BigSmallTurboValue:
                    return Enumeration.GetAll<BigSmallBetChoice>();

                case Game.OddEvenValue:
                case Game.OddEvenTurboValue:
                case Game.SpecialSeDieValue:
                    return Enumeration.GetAll<OddEvenBetChoice>();

                case Game.SeDieValue:
                    return Enumeration.GetAll<SeDieBetChoice>();

                case Game.FishPrawnCrabValue:
                    return Enumeration.GetAll<FishPrawnCrabBetOdds>();

                case Game.DragonTigerValue:
                    return Enumeration.GetAll<DragonTigerBetChoice>();

                case Game.KenoProMaxValue:
                case Game.KenoProVietValue:
                case Game.KenoMaxValue:
                case Game.KenoMax2Value:
                case Game.KenoMiniValue:
                case Game.KenoMini2Value:
                case Game.KenoEastValue:
                case Game.KenoWestValue:
                case Game.KenoSouthValue:
                case Game.KenoNorthValue:
                    return Enumeration.GetAll<KenoBetChoice>();

                case Game.MiniSeDieValue:
                    return Enumeration.GetAll<MiniSeDieBetChoice>();

                case Game.BaccaratValue:
                    return Enumeration.GetAll<BaccaratBetChoice>();

                case Game.RouletteValue:
                    return Enumeration.GetAll<RouletteBetChoice>();

                case Game.VipSeDieValue:
                    return Enumeration.GetAll<VipSeDieBetChoice>();

                case Game.VipFishPrawnCrabValue:
                    return Enumeration.GetAll<VipFishPrawnCrabBetOdds>();

                case Game.BlackJackValue:
                    return Enumeration.GetAll<BlackjackBetChoice>();

                case Game.SicBoValue:
                    return Enumeration.GetAll<SicBoBetOdds>();

                case Game.VipBaccaratValue:
                    return Enumeration.GetAll<VipBaccaratBetChoice>();

                case Game.VipSeDieWarValue:
                    return Enumeration.GetAll<VipSeDieWarBetChoice>();


                case Game.LotteryValue:
                    return Enumeration.GetAll<LotteryBetChoice>();

                case Game.ThreeCardsValue:
                    return Enumeration.GetAll<ThreeCardsBetChoice>();

                case Game.KenoMatch1Value:
                case Game.KenoMatch2Value:
                    return Enumeration.GetAll<KenoMatchBetOdds>();

                case Game.KenoPower1Value:
                case Game.KenoPower2Value:
                    return Enumeration.GetAll<KenoPowerBetChoice>();

                default: return default;
            }
        }
    }
}