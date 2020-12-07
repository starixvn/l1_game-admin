using System.Collections.Generic;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("vipsediewar_phiens")]
    public class VipSeDieWarRoundEntity : BaseRoundEntity
    {
        public VipSeDieWarRoundResultEntity Result { get; set; }

        public Dictionary<string, bool> SettlementResult { get; set; }
    }

    public class VipSeDieWarHandResultEntity
    {
        [BsonElement("red1")]
        public bool FirstChip { get; set; }

        [BsonElement("red2")]
        public bool SecondChip { get; set; }

        [BsonElement("red3")]
        public bool ThirdChip { get; set; }

        [BsonElement("red4")]
        public bool FourthChip { get; set; }
    }

    public class VipSeDieWarRoundResultEntity
    {
        public VipSeDieWarHandResultEntity Player { get; set; }

        public VipSeDieWarHandResultEntity Banker { get; set; }
    }
}