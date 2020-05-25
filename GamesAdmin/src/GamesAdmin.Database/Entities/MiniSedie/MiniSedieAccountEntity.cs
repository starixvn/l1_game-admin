using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities.MiniSedie
{
    [BsonCollection("minisedie_users")]
    public class MiniSedieAccountEntity : MiniGameBaseEntity
    {
        public double Red { get; set; }
        public double RedLost { get; set; }
        public double RedPlay { get; set; }
        public double RedWin { get; set; }
        public string Uid { get; set; }
    }
}
