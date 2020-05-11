using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("kenopromax_users")]
    public class KenoProMaxAccountEntity : MiniGameBaseEntity
    {
        public double Red { get; set; }
        public double RedLost { get; set; }
        public double RedPlay { get; set; }
        public double RedWin { get; set; }
        public string Uid { get; set; }
    }
}
