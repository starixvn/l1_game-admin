using System.Collections.Generic;
using GamesAdmin.Core.Enumeration.BetChoices;

namespace GamesAdmin.Core.Enumeration
{
    public static class BetChoiceMapper
    {
        private static Dictionary<string, IEnumerable<Enumeration>> gameBetChoices = new Dictionary<string, IEnumerable<Enumeration>>
        {
            {GameType.BigSmallValue, Enumeration.GetAll<BigSmallBetChoice>() },
            {GameType.OddEvenValue, Enumeration.GetAll<OddEvenBetChoice>() },
            {GameType.SeDieValue, Enumeration.GetAll<SeDieBetChoice>() },
            {GameType.FishPrawnCrabValue, Enumeration.GetAll<FishPrawnCrabBetChoice>() },
            {GameType.DragonTigerValue, Enumeration.GetAll<DragonTigerBetChoice>() },
            {GameType.KenoProMaxValue, Enumeration.GetAll<KenoBetChoice>() },
            {GameType.MiniSeDieValue, Enumeration.GetAll<MiniSeDieBetChoice>() },
            {GameType.BaccaratValue, Enumeration.GetAll<BaccaratBetChoice>() },
            {GameType.RouletteValue, Enumeration.GetAll<RouletteBetChoice>() },
            {GameType.VipSeDieValue, Enumeration.GetAll<VipSeDieBetChoice>() },
            {GameType.BlackJackValue, Enumeration.GetAll<BlackjackBetChoice>() },
            {GameType.VipFishPrawnCrabValue, Enumeration.GetAll<VipFishPrawnCrabBetChoice>() },
        };

        private static List<string> KenoTypes = new List<string>
        {
             GameType.KenoProVietValue,
             GameType.KenoMaxValue,
             GameType.KenoMax2Value,
             GameType.KenoMiniValue,
             GameType.KenoMini2Value,
             GameType.KenoEastValue,
             GameType.KenoWestValue,
             GameType.KenoSouthValue,
             GameType.KenoNorthValue
        };

        public static IEnumerable<Enumeration> Map(GameType gameType)
        {
            var game = ConvertGameTypeValue(gameType);

            if (gameBetChoices.ContainsKey(game))
            {
                return gameBetChoices[game];
            }

            return default;
        }

        public static string ConvertGameTypeValue(GameType gameType)
        {
            if (KenoTypes.Contains(gameType.Value))
            {
                return GameType.KenoProMaxValue;
            }

            switch (gameType.Value)
            {
                case GameType.BigSmallTurboValue:
                    return GameType.BigSmallValue;

                case GameType.OddEvenTurboValue:
                case GameType.SpecialSeDieValue:
                    return GameType.OddEvenValue;

                default: return gameType.Value;
            }
        }
    }
}