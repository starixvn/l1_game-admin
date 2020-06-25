using System.Collections.Generic;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson;

namespace GamesAdmin.Database.Entities.Keno
{
    [BsonCollection("kenoproviet_ones")]
    public class KenoProVietOnesEntity : BaseBetEntity
    {
        public decimal Big { get; set; }

        public decimal Small { get; set; }

        public decimal Odd { get; set; }

        public decimal Even { get; set; }

        public decimal Gold { get; set; }

        public decimal Earth { get; set; }

        public decimal Fire { get; set; }

        public decimal Water { get; set; }

        public decimal Wood { get; set; }

        public Dictionary<string, decimal> Odds { get; set; }

        public ObjectId UserId { get; set; }
    }
}
