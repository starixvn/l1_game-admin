using System.Collections.Generic;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities.ShootingBlitz
{
    [BsonCollection("ShootingBlitz_team")]
    public class ShootingBlitzTeamEntity : MiniGameBaseEntity
    {
        [BsonElement("teamId")]
        public int TeamId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("playerList")]
        public List<ShootingBlitzPlayerEntity> PlayerList { get; set; }
    }

    public class ShootingBlitzPlayerEntity
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("playerNumber")]
        public int PlayerNumber { get; set; }
    }
}

