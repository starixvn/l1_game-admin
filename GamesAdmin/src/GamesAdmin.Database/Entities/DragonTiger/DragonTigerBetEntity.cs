using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("minidragontiger_cuocs")]
    public class DragonTigerBetEntity : BaseBetEntity
    {
        public int Dragon { get; set; }

        public int Tie { get; set; }

        public int Tiger { get; set; }
    }
}