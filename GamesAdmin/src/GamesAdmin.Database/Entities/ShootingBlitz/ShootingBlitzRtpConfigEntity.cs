using System.Collections.Generic;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities.ShootingBlitz
{
    [BsonCollection("ShootingBlitz_rtpConfig")]
    public class ShootingBlitzRtpConfigEntity : MiniGameBaseEntity
    {
        [BsonElement("rtp")]
        public double Rtp { get; set; }

        [BsonElement("bombCount")]
        public List<BombCountConfigEntity> BombCount { get; set; }

        [BsonElement("multiplierSteps")]
        public List<MultiplierStepsConfigEntity> MultiplierSteps { get; set; }
    }

    public class BombCountConfigEntity
    {
        [BsonElement("bombCount")]
        public int BombCount { get; set; }

        [BsonElement("probability")]
        public double Probability { get; set; }
    }

    public class MultiplierStepsConfigEntity
    {
        [BsonElement("mode")]
        public int Mode { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("starPlayerRate")]
        public double StarPlayerRate { get; set; }

        [BsonElement("starPosition")]
        public List<int> StarPosition { get; set; }

        [BsonElement("multiplier")]
        public List<MultiplierItemEntity> Multiplier { get; set; }
    }

    public class MultiplierItemEntity
    {
        [BsonElement("picks")]
        public int Picks { get; set; }

        [BsonElement("pays")]
        public double Pays { get; set; }
    }
}

