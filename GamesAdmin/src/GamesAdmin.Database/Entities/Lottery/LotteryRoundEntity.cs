using System.Collections.Generic;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("lottery_phiens")]
    public class LotteryRoundEntity : BaseRoundEntity
    {
        public LotteryResultEntity Result { get; set; }

        public LotterySettlementResultEntity SettlementResult { get; set; }
    }

    public class LotteryResultEntity
    {
        public IEnumerable<string> Special { get; set; }
        public IEnumerable<string> First { get; set; }
        public IEnumerable<string> Second { get; set; }
        public IEnumerable<string> Third { get; set; }
        public IEnumerable<string> Fourth { get; set; }
        public IEnumerable<string> Fifth { get; set; }
        public IEnumerable<string> Sixth { get; set; }
        public IEnumerable<string> Seventh { get; set; }
    }

    public class LotterySettlementResultEntity
    {
        [BsonElement("4Sets")]
        public IEnumerable<string> FourSets { get; set; }

        public string Special { get; set; }

        public string Head { get; set; }

        public string Tail { get; set; }

        public Dictionary<string, bool> Others { get; set; }

        public IEnumerable<string> TwentySevenSets { get; set; }

        public LotteryOccurrencesEntity Occurrences { get; set; }
    }

    public class LotteryOthersSettlementResultEntity
    {
        public bool Odd { get; set; }
        public bool Even { get; set; }
        public bool Big { get; set; }
        public bool Small { get; set; }
    }

    public class LotteryOccurrencesEntity
    {
        public Dictionary<string, byte> All { get; set; }
        public Dictionary<string, byte> Seventh { get; set; }
    }
}