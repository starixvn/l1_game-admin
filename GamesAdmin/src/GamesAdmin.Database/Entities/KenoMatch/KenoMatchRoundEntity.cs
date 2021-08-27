using System;
using System.Collections.Generic;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    public class KenoMatchRoundEntity : BaseRoundEntity
    {
        [BsonElement("firstHalf")]
        public KenoMatchPeriodEntity HalfTime { get; set; }
        public KenoMatchPeriodEntity FullTime { get; set; }

        public Dictionary<string, decimal> Odds { get; set; }
    }

    public class KenoMatchPeriodEntity
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime Time { get; set; }

        [BsonElement("sum")]
        public int Total { get; set; }

        public KenoMatchPeriodResultEntity Result { get; set; }

        public KenoMatchPeriodSettlementResultEntity SettlementResult { get; set; }
    }

    public class KenoMatchPeriodResultEntity
    {
        public IEnumerable<int> Blue { get; set; }
        public IEnumerable<int> Red { get; set; }
    }

    public class KenoMatchPeriodSettlementResultEntity
    {
        public bool Blue { get; set; }
        public bool Red { get; set; }
        public bool Over { get; set; }
        public bool Under { get; set; }
        public bool Odd { get; set; }
        public bool Even { get; set; }
    }

    [BsonCollection("kenomatch1_phiens")]
    public class KenoMatch1RoundEntity : KenoMatchRoundEntity
    {
    }

    [BsonCollection("kenomatch2_phiens")]
    public class KenoMatch2RoundEntity : KenoMatchRoundEntity
    {
    }
}