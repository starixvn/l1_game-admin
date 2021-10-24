using System.Collections.Generic;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("currency_group_games")]
    public class CurrencyGroupGameEntity : Document
    {
        public ObjectId CurrencyGroupId { get; set; }

        public int GameId { get; set; }

        public bool Enabled { get; set; }

        public double MinBet { get; set; }

        public double MaxBet { get; set; }

        public Dictionary<string, double> MaxBetPerChoices { get; set; }

        public string[] Chips { get; set; }
    }
}