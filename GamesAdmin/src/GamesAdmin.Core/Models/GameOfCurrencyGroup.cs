using System.Collections.Generic;

namespace GamesAdmin.Core.Models
{
    public class CurrencyGroupGame
    {
        public CurrencyGroupGame(string currencyGroupId, int gameId, bool enabled, double minbet, double maxbet, Dictionary<string, double> maxBetPerChoices, string[] chips)
        {
            CurrencyGroupId = currencyGroupId;
            GameId = gameId;
            Enabled = enabled;
            MinBet = minbet;
            MaxBet = maxbet;
            MaxBetPerChoices = maxBetPerChoices;
            Chips = chips;
        }

        public string CurrencyGroupId { get; }

        public int GameId { get; }

        public bool Enabled { get; }

        public double MinBet { get; }

        public double MaxBet { get; }

        public Dictionary<string, double> MaxBetPerChoices { get; }

        public string[] Chips { get; }
    }
}