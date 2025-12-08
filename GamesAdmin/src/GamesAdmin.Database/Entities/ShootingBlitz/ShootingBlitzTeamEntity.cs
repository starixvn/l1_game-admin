using System.Collections.Generic;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities.ShootingBlitz
{
    [BsonCollection("ShootingBlitz_teams")]
    public class ShootingBlitzTeamEntity : MiniGameBaseEntity
    {
        [BsonElement("teamId")]
        public int TeamId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        /// <summary>
        /// 是否啟用此 Team（預設 true，null 視為 true）
        /// </summary>
        [BsonElement("isEnabled")]
        public bool? IsEnabled { get; set; } = true;

        [BsonElement("starPlayList")]
        public List<ShootingBlitzPlayerEntity> StarPlayList { get; set; }
    }

    public class ShootingBlitzPlayerEntity
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("playerNumber")]
        public int PlayerNumber { get; set; }
    }
}

