using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("fantan_rounds")]
    public class FanTanRoundEntity : BaseRoundEntity
    {
        public byte Result { get; set; }

        public FanTanSettlementResultEntity SettlementResult { get; set; }
    }

    public class FanTanSettlementResultEntity
    {
        public bool Odd { get; set; }
        public bool Even { get; set; }

        public bool Fan1 { get; set; }
        public bool Fan2 { get; set; }
        public bool Fan3 { get; set; }
        public bool Fan4 { get; set; }

        public bool Kwok12 { get; set; }
        public bool Kwok23 { get; set; }
        public bool Kwok34 { get; set; }
        public bool Kwok41 { get; set; }

        public bool Chun123 { get; set; }
        public bool Chun124 { get; set; }
        public bool Chun134 { get; set; }
        public bool Chun234 { get; set; }
    }
}