using System.Collections.Generic;
using GamesAdmin.Core.Enumeration.BetChoices;

namespace GamesAdmin.Core.Enumeration
{
    public static class BetChoiceMapper
    {
        private static Dictionary<string, IEnumerable<Enumeration>> gameBetChoices = new Dictionary<string, IEnumerable<Enumeration>>
        {
            {Game.BigSmallValue, Enumeration.GetAll<BigSmallBetChoice>() },
            {Game.OddEvenValue, Enumeration.GetAll<OddEvenBetChoice>() },
            {Game.SeDieValue, Enumeration.GetAll<SeDieBetChoice>() },
            {Game.FishPrawnCrabValue, Enumeration.GetAll<FishPrawnCrabBetChoice>() },
            {Game.DragonTigerValue, Enumeration.GetAll<DragonTigerBetChoice>() },
            {Game.KenoProMaxValue, Enumeration.GetAll<KenoBetChoice>() },
            {Game.MiniSeDieValue, Enumeration.GetAll<MiniSeDieBetChoice>() },
            {Game.BaccaratValue, Enumeration.GetAll<BaccaratBetChoice>() },
            {Game.RouletteValue, Enumeration.GetAll<RouletteBetChoice>() },
            {Game.VipSeDieValue, Enumeration.GetAll<VipSeDieBetChoice>() },
            {Game.BlackJackValue, Enumeration.GetAll<BlackjackBetChoice>() },
            {Game.VipFishPrawnCrabValue, Enumeration.GetAll<VipFishPrawnCrabBetChoice>() },
            {Game.SicBoValue, Enumeration.GetAll<SicBoBetChoice>() },
            {Game.VipBaccaratValue, Enumeration.GetAll<VipBaccaratBetChoice>() },
            {Game.VipSeDieWarValue, Enumeration.GetAll<VipSeDieWarBetChoice>() },
            {Game.LotteryValue, Enumeration.GetAll<LotteryBetChoice>() },
            {Game.ThreeCardsValue, Enumeration.GetAll<ThreeCardsBetChoice>() },
            {Game.CasinoHoldemPokerValue, Enumeration.GetAll<CasinoHoldemPokerBetChoice>() },
            {Game.KenoMatch1Value, Enumeration.GetAll<KenoMatchBetChoice>() },
            {Game.KenoMatch2Value, Enumeration.GetAll<KenoMatchBetChoice>() },
            {Game.KenoPower1Value, Enumeration.GetAll<KenoPowerBetChoice>() },
            {Game.KenoPower2Value, Enumeration.GetAll<KenoPowerBetChoice>() },
            {Game.BitcoinMasterValue, Enumeration.GetAll<BitcoinMasterBetChoice>() },
            {Game.TexasHoldemPokerValue, Enumeration.GetAll<TexasHoldemPokerBetChoice>() },
        };

        private static List<string> KenoTypes = new List<string>
        {
             Game.KenoProVietValue,
             Game.KenoMaxValue,
             Game.KenoMax2Value,
             Game.KenoMiniValue,
             Game.KenoMini2Value,
             Game.KenoEastValue,
             Game.KenoWestValue,
             Game.KenoSouthValue,
             Game.KenoNorthValue
        };

        public static IEnumerable<Enumeration> Map(Game gameType)
        {
            var game = ConvertGameTypeValue(gameType);

            if (gameBetChoices.ContainsKey(game))
            {
                return gameBetChoices[game];
            }

            return default;
        }

        public static string ConvertGameTypeValue(Game gameType)
        {
            if (KenoTypes.Contains(gameType.Value))
            {
                return Game.KenoProMaxValue;
            }

            switch (gameType.Value)
            {
                case Game.BigSmallTurboValue:
                    return Game.BigSmallValue;

                case Game.OddEvenTurboValue:
                case Game.SpecialSeDieValue:
                    return Game.OddEvenValue;

                default: return gameType.Value;
            }
        }
    }
}