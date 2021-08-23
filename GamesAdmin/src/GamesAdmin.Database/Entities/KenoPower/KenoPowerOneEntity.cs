using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    public class KenoPowerOneEntity : BaseBetEntity
    {
        public decimal Player { get; set; }
        public decimal Banker { get; set; }
        public decimal Big { get; set; }
        public decimal Small { get; set; }
        public decimal Odd { get; set; }
        public decimal Even { get; set; }
        public decimal Red { get; set; }
        public decimal Orange { get; set; }
        public decimal Yellow { get; set; }
        public decimal Green { get; set; }
        public decimal Blue { get; set; }
        public decimal Indigo { get; set; }
        public decimal Violet { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal Refund { get; set; }

        public Dictionary<string, bool> Odds { get; set; }
    }

    [BsonCollection("kenopower1_ones")]
    public class KenoPower1OneEntity : KenoPowerOneEntity
    {
    }

    [BsonCollection("kenopower2_ones")]
    public class KenoPower2OneEntity : KenoPowerOneEntity
    {

    }
}