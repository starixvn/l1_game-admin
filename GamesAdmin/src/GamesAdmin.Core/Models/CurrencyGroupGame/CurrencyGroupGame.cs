using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Models.GameConfigs;

namespace GamesAdmin.Core.Models
{
    public class CurrencyGroupGame
    {
        public CurrencyGroupGame(
            string groupId,
            int gameId,
            string gameName,
            bool enabled,
            double minbet,
            double maxbet,
            Dictionary<string, double> maxBetPerChoices = null,
            string[] chips = null,
            double[] betLevels = null,
            StakeLevel[] stakeLevels = null,
            double[] coinValues = null,
            int multiplier = 0,
            double maxWinPerRound = 0,
            double? defaultCoinValue = null,
            double? defaultBetLevel = null,
            IEnumerable<int> otherGameIdsInGroup = null)
        {
            GroupId = groupId;
            GameId = gameId;
            GameName = gameName;
            Enabled = enabled;
            MinBet = minbet;
            MaxBet = maxbet;
            MaxBetPerChoices = maxBetPerChoices;
            Chips = chips;
            BetLevels = betLevels;
            StakeLevels = stakeLevels;
            CoinValues = coinValues;
            Multiplier = multiplier;
            MaxWinPerRound = maxWinPerRound;
            DefaultCoinValue = defaultCoinValue;
            DefaultBetLevel = defaultBetLevel;
            OtherGameIdsInGroup = otherGameIdsInGroup ?? Enumerable.Empty<int>();
        }

        public string GroupId { get; private set; }

        public int GameId { get; set; }

        public string GameName { get; set; }

        public bool Enabled { get; set; }

        public double MinBet { get; }

        public double MaxBet { get; }

        public Dictionary<string, double> MaxBetPerChoices { get; }

        public string[] Chips { get; }

        public double[] BetLevels { get; }

        public StakeLevel[] StakeLevels { get; }

        public double[] CoinValues { get; }
        public double? DefaultCoinValue { get; set; }
        public double? DefaultBetLevel { get; set; }
        public int Multiplier { get; }
        public double MaxWinPerRound { get; }

        public IEnumerable<int> OtherGameIdsInGroup { get; }

        public void SetGroupId(string groupId)
        {
            GroupId = groupId;
        }
    }
}