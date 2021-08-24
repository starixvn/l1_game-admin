using System;
using System.Collections.Generic;
using System.Text;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    public class KenoPowerRoundEntity : BaseRoundEntity
    {
        public KenoPowerResultEntity Result { get; set; }
        public KenoPowerSettlementResultEntity SettlementResult { get; set; }
    }

    public class KenoPowerResultEntity
    {
        public IEnumerable<byte> Player { get; set; }
        public IEnumerable<byte> Banker { get; set; }
    }

    public class KenoPowerSettlementResultEntity
    {
        public bool Player { get; set; }
        public bool Banker { get; set; }
        public bool Big { get; set; }
        public bool Small { get; set; }
        public bool Odd { get; set; }
        public bool Even { get; set; }
        public bool Red { get; set; }
        public bool Orange { get; set; }
        public bool Yellow { get; set; }
        public bool Green { get; set; }
        public bool Blue { get; set; }
        public bool Indigo { get; set; }
        public bool Violet { get; set; }
        public bool PlayerBig { get; set; }
        public bool PlayerSmall { get; set; }
        public bool BankerBig { get; set; }
        public bool BankerSmall { get; set; }
    }


    [BsonCollection("kenopower1_phiens")]
    public class KenoPower1RoundEntity : KenoPowerRoundEntity
    {

    }

    [BsonCollection("kenopower2_phiens")]
    public class KenoPower2RoundEntity : KenoPowerRoundEntity
    {

    }
}
