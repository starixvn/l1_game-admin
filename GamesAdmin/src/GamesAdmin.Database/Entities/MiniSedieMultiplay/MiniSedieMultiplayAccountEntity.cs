using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities.MiniSedie
{
    [BsonCollection("minisedie2_users")]
    public class MiniSedie2AccountEntity : MiniSedieMultiplayAccountEntity
    {
    }

    public class MiniSedie3AccountEntity : MiniSedieMultiplayAccountEntity
    {
    }

    public class MiniSedie4AccountEntity : MiniSedieMultiplayAccountEntity
    {
    }

    public class MiniSedieMultiplayAccountEntity : MiniGameBaseEntity
    {
        public double Red { get; set; }
        public double RedLost { get; set; }
        public double RedPlay { get; set; }
        public double RedWin { get; set; }
        public string Uid { get; set; }
    }
}
