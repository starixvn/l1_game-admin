using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("baucua_users")]
    public class BauCuaAccountEntity : Document
    {
        public int red { get; set; }
        public int red_lost { get; set; }
        public int redPlay { get; set; }
        public int xu { get; set; }
        public int xu_lost { get; set; }
        public int xuPlay { get; set; }
        public int thuong { get; set; }
        public string uid { get; set; }
    }
}
