using System.Collections.Generic;
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
            StakeLevel[] stakeLevels = null,
            double[] coinValues = null,
            int multiplier = 0,
            double maxWinPerRound = 0,
            double? defaultCoinValue = null)
        {
            GroupId = groupId;
            GameId = gameId;
            GameName = gameName;
            Enabled = enabled;
            MinBet = minbet;
            MaxBet = maxbet;
            MaxBetPerChoices = maxBetPerChoices;
            Chips = chips;
            StakeLevels = stakeLevels;
            CoinValues = coinValues;
            Multiplier = multiplier;
            MaxWinPerRound = maxWinPerRound;
            DefaultCoinValue = defaultCoinValue;
        }

        public string GroupId { get; }

        public int GameId { get; }

        public string GameName { get; }

        public bool Enabled { get; }

        public double MinBet { get; }

        public double MaxBet { get; }

        public Dictionary<string, double> MaxBetPerChoices { get; }

        public string[] Chips { get; }

        public StakeLevel[] StakeLevels { get; }

        public double[] CoinValues { get; }
        public double? DefaultCoinValue { get; set; }
        public int Multiplier { get; }
        public double MaxWinPerRound { get; }
    }
}