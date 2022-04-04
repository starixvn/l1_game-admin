using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("currency_group_games")]
    public class CurrencyGroupGameEntity : Document
    {
        public ObjectId GroupId { get; set; }

        public int GameId { get; set; }

        public string GameName { get; set; }

        public bool Enabled { get; set; }

        public double MinBet { get; set; }

        public double MaxBet { get; set; }

        public Dictionary<string, double> MaxBetPerChoices { get; set; }

        public string[] Chips { get; set; }

        public StakeLevel[] StakeLevels { get; set; }

        public double[] BetLevels { get; set; }
        public double[] CoinValues { get; set; }
        public int Multiplier { get; set; }
        public double MaxWinPerRound { get; set; }
    }
}