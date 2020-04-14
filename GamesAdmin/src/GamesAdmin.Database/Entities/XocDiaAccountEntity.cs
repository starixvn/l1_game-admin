using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("xocxoc_users")]
    public class XocDiaAccountEntity : MiniGameBaseEntity
    {
        public double Red { get; set; }
        public int Red_lost { get; set; }
        public int RedPlay { get; set; }
        public int Xu { get; set; }
        public int Xu_lost { get; set; }
        public int XuPlay { get; set; }
        public int Thuong { get; set; }
        public string Uid { get; set; }
    }
}
