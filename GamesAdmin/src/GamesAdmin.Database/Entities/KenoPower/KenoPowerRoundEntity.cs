using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    public class KenoPowerRoundEntity : BaseRoundEntity
    {
        public KenoPowerResultEntity Result { get; set; }
        public KenoPowerSettlementResultEntity SettlementResult { get; set; }

        public bool Settled { get; set; }
    }

    public class KenoPowerResultEntity
    {
        public IEnumerable<byte> Blue { get; set; }
        public IEnumerable<byte> Red { get; set; }
    }

    public class KenoPowerSettlementResultEntity
    {
        public bool Blue { get; set; }
        public bool Red { get; set; }
        public bool Big { get; set; }
        public bool Small { get; set; }
        public bool Odd { get; set; }
        public bool Even { get; set; }
        public bool BlueBig { get; set; }
        public bool BlueSmall { get; set; }
        public bool RedBig { get; set; }
        public bool RedSmall { get; set; }
    }

    [BsonCollection("kenopower_rounds")]
    public class KenoPower1RoundEntity : KenoPowerRoundEntity
    {
    }

    [BsonCollection("kenopower2_rounds")]
    public class KenoPower2RoundEntity : KenoPowerRoundEntity
    {
    }
}