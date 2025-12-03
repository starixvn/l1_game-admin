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

        [BsonElement("multiplierSteps")]
        public List<MultiplierStepsConfigEntity> MultiplierSteps { get; set; }
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

        /// <summary>
        /// Pays array where index+1 = picks (e.g., pays[0] = picks 1, pays[1] = picks 2)
        /// </summary>
        [BsonElement("pays")]
        public List<double> Pays { get; set; }
    }
}

