using System;
using System.Collections.Generic;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    public class KenoSoccerRoundEntity : BaseRoundEntity
    {
        public KenoSoccerPeriodEntity HalfTime { get; set; }
        public KenoSoccerPeriodEntity FullTime { get; set; }

        public Dictionary<string, decimal> Odds { get; set; }
    }

    public class KenoSoccerPeriodEntity
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime Time { get; set; }

        [BsonElement("sum")]
        public int Total { get; set; }

        public KenoSoccerPeriodResultEntity Result { get; set; }

        public KenoSoccerPeriodSettlementResultEntity SettlementResult { get; set; }
    }

    public class KenoSoccerPeriodResultEntity
    {
        public IEnumerable<int> Home { get; set; }
        public IEnumerable<int> Away { get; set; }
    }

    public class KenoSoccerPeriodSettlementResultEntity
    {
        public bool Home { get; set; }
        public bool Away { get; set; }
        public bool Over { get; set; }
        public bool Under { get; set; }
        public bool Odd { get; set; }
        public bool Even { get; set; }
    }

    [BsonCollection("mydinh_phiens")]
    public class KenoSoccerMyDinhRoundEntity : KenoSoccerRoundEntity
    {
    }

    [BsonCollection("hangday_phiens")]
    public class KenoSoccerHangDayRoundEntity : KenoSoccerRoundEntity
    {
    }

    [BsonCollection("thongnhat_phiens")]
    public class KenoSoccerThongNhatRoundEntity : KenoSoccerRoundEntity
    {
    }
}