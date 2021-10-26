using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;

namespace GamesAdmin.Core.Models
{
    public class CurrencyGroupGame
    {
        public CurrencyGroupGame(string currencyGroupId, int gameId, string gameName, bool enabled, double minbet, double maxbet, Dictionary<string, double> maxBetPerChoices, string[] chips, StakeLevel[] stakeLevels)
        {
            CurrencyGroupId = currencyGroupId;
            GameId = gameId;
            GameName = gameName;
            Enabled = enabled;
            MinBet = minbet;
            MaxBet = maxbet;
            MaxBetPerChoices = maxBetPerChoices;
            Chips = chips;
            StakeLevels = stakeLevels;
        }

        public string CurrencyGroupId { get; }

        public int GameId { get; }

        public string GameName { get; }

        public bool Enabled { get; }

        public double MinBet { get; }

        public double MaxBet { get; }

        public Dictionary<string, double> MaxBetPerChoices { get; }

        public string[] Chips { get; }

        public StakeLevel[] StakeLevels { get; }
    }
}